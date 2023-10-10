using AutoMapper;
using TennisClub.Api.Models.Cosmos.Containers;
using TennisClub.Api.Models.Input;
using TennisClub.Api.Models.Output;

namespace TennisClub.Api.Services;

public class TournamentService
{
    private readonly BaseCosmosRepository _repo;
    private readonly IMapper _mapper;
    private readonly ImageStoreService _imageStoreService;
    private readonly IConfiguration _configuration;

    public TournamentService(BaseCosmosRepository repo, IMapper mapper, ImageStoreService imageStoreService, IConfiguration configuration)
        => (_repo, _mapper, _imageStoreService, _configuration) = (repo, mapper, imageStoreService, configuration);

    public async Task<TournamentOutputModel> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        var tournament = await _repo.GetAsync<Tournament>(id, cancellationToken);

        return _mapper.Map<TournamentOutputModel>(tournament, options => options.AfterMap(async (_, outputModel) =>
        {
            outputModel.Logo = await _imageStoreService.GetBase64StringImageFromUrlAsync(tournament.LogoUrl, cancellationToken);
            foreach (var item in outputModel.Courts)
            {
                item.Picture = await _imageStoreService.GetBase64StringImageFromUrlAsync(tournament.Courts.Single(c => c.Name == item.Name).PictureUrl, cancellationToken);
            }
        }));
    }

    public async Task<IEnumerable<TournamentOutputModel>> GetAll(CancellationToken cancellationToken = default)
    {
        var tournaments = await _repo.GetAllAsync<Tournament>(cancellationToken);

        return _mapper.Map<IEnumerable<TournamentOutputModel>>(tournaments);
    }

    public async Task<IEnumerable<TournamentBasicOutputModel>> GetAllTournamentsBasicInfoAsync(CancellationToken cancellationToken = default)
    {
        var queryText = @"SELECT VALUE { Id: t.id, Name: t.Name, Category: t.Category }
                          FROM tournaments t";

        return await _repo.GetItemsFromQueryAsync<Tournament, TournamentBasicOutputModel>(queryText, cancellationToken: cancellationToken); ;
    }

    public async Task<IEnumerable<TournamentWinnerOutputModel>> GetPastWinnersAsync(string tournamentId, CancellationToken cancellationToken = default)
    {
        return await _repo.GetItemsFromQueryAsync<Match, TournamentWinnerOutputModel>(
            PastWinnersQuery,
            new() { { "@tournamentId", tournamentId }, { "@finalPhase", "final" } },
            cancellationToken);
    }

    public async Task<IEnumerable<MatchOutputModel>> GetMatchesByTournamentAndYearAsync(string tournamentId, int year, CancellationToken cancellationToken = default)
    {
        var tournamentMatchesQuery = @"SELECT *
                                       FROM matches m
                                       WHERE m.TournamentId = @tournamentId AND DateTimePart('yyyy', m.Date) = @year";

        var tournamentMatches = await _repo.GetItemsFromQueryAsync<Match>(
            tournamentMatchesQuery,
            new() { { "@tournamentId", tournamentId }, { "@year", year } },
            cancellationToken);

        return _mapper.Map<IEnumerable<MatchOutputModel>>(tournamentMatches);
    }

    public async Task<IEnumerable<PlayerBasicOutputModel>> GetTournamentRecordAsync(string tournamentId, CancellationToken cancellationToken = default)
    {
        var pastWinners = await _repo.GetItemsFromQueryAsync<Match, TournamentWinnerOutputModel>(
            PastWinnersQuery,
            new() { { "@tournamentId", tournamentId }, { "@finalPhase", "final" } },
            cancellationToken);

        if (!pastWinners.Any())
            return null;

        return new[] { pastWinners.GroupBy(w => w.Player).OrderByDescending(gr => gr.Count()).First().Key, pastWinners.First().Player };

    }

    public async Task<string> InsertTournamentAsync(TournamentInputModel input, CancellationToken cancellationToken = default)
    {
        var tournament = _mapper.Map<Tournament>(input, options => options.AfterMap(async (_, tournament) =>
        {
            tournament.LogoUrl = await _imageStoreService.StoreImageAsync(input.Logo, cancellationToken: cancellationToken);
            foreach (var item in tournament.Courts)
            {
                item.PictureUrl = await _imageStoreService.StoreImageAsync(input.Courts.Single(c => c.Name == item.Name).Picture, _configuration["ImageStore:CourtImages"]);
            }
        }));

        return await _repo.InsertAsync<Tournament>(tournament, cancellationToken);
    }

    public async Task UpdateTournamentAsync(string id, TournamentInputModel inputModel, CancellationToken cancellationToken = default)
    {
        var tournament = _mapper.Map<Tournament>(inputModel);

        await _repo.UpdateAsync<Tournament>(id, tournament, cancellationToken);
    }

    public async Task RemoveTournamentAsync(string id, CancellationToken cancellationToken = default)
        => await _repo.RemoveAsync<Tournament>(id, cancellationToken);

    private static string PastWinnersQuery => @"SELECT VALUE { 
                                                  Player: { 
                                                      Id: IIF(m.WinnerId = m.FirstPlayer.PlayerId, m.FirstPlayer.PlayerId, m.SecondPlayer.PlayerId),
                                                      FirstName: IIF(m.WinnerId = m.FirstPlayer.PlayerId, m.FirstPlayer.FirstName, m.SecondPlayer.FirstName),
                                                      LastName: IIF(m.WinnerId = m.FirstPlayer.PlayerId, m.FirstPlayer.LastName, m.SecondPlayer.LastName)
                                                  },
                                                  Year: DateTimePart('yyyy', m.Date)
                                                }
                                                FROM matches m
                                                WHERE m.TournamentId = @tournamentId AND LOWER(m.Phase) = @finalPhase
                                                ORDER BY m.DATE DESC";
}
