using AutoMapper;
using TennisClub.Api.Models.Cosmos.Containers;
using TennisClub.Api.Models.Input;
using TennisClub.Api.Models.Output;

namespace TennisClub.Api.Services;

public class PlayerService
{
    private readonly BaseCosmosRepository _repo;
    private readonly ImageStoreService _imageStoreService;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public PlayerService(BaseCosmosRepository repo, ImageStoreService imageStoreService, IMapper mapper, IConfiguration configuration)
        => (_repo, _imageStoreService, _mapper, _configuration) = (repo, imageStoreService, mapper, configuration);

    public async Task<PlayerOutputModel> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        var player = await _repo.GetAsync<Player>(id, cancellationToken);

        var playerLatestRankQuery = @"SELECT VALUE r.RankNumber
                                      FROM weeklyRanks wr
                                      JOIN r IN wr.Ranks
                                      WHERE r.PlayerId = @playerId
                                      ORDER BY wr.Year DESC, wr.Week DESC
                                      OFFSET 0 LIMIT 1";

        var rank = await _repo.GetItemsFromQueryAsync<WeeklyRank, int>(playerLatestRankQuery, new() { { "@playerId", id } }, cancellationToken);

        return _mapper.Map<Player, PlayerOutputModel>(player, opt => opt.AfterMap(async (Player player, PlayerOutputModel outputModel) =>
        {
            outputModel.Rank = rank.Any() ? rank.First() : 0;
            outputModel.Picture = await _imageStoreService.GetBase64StringImageFromUrlAsync(player.PictureUrl, cancellationToken);
            outputModel.StandingPicture = await _imageStoreService.GetBase64StringImageFromUrlAsync(player.StandingPictureUrl, cancellationToken);
        }));

    }

    public async Task<IEnumerable<PlayerOutputModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var players = await _repo.GetAllAsync<Player>(cancellationToken);

        // TODO: maybe players ranks should be queried/calculated, similar as in above method, and also calculate map rank in AfterMap

        return _mapper.Map<IEnumerable<PlayerOutputModel>>(players);
    }


    public async Task<IEnumerable<PlayerBasicOutputModel>> GetAllPlayersBasicInfoAsync(CancellationToken cancellationToken = default)
    {
        var queryText = @"SELECT VALUE {Id: p.id, FirstName: p.FirstName, LastName: p.LastName}
                        FROM players p";

        return await _repo.GetItemsFromQueryAsync<Player, PlayerBasicOutputModel>(queryText, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<MatchOutputModel>> GetFinalMatchesAsync(string id, CancellationToken cancellationToken = default)
    {
        var predicate = @"WHERE (m.FirstPlayer.PlayerId = @playerId OR m.SecondPlayer.PlayerId = @playerId) AND LOWER(m.Phase) = @phase";

        return await GetMatchesByPredicateAsync(
            predicate,
            new() { { "@playerId", id }, { "@phase", "final" } },
            cancellationToken);
    }

    public async Task<IEnumerable<MatchOutputModel>> GetRecentMachesAsync(string id, int takeCount, CancellationToken cancellationToken = default)
    {
        var predicate = @"WHERE m.FirstPlayer.PlayerId = @playerId OR m.SecondPlayer.PlayerId = @playerId
                          ORDER BY m.Date DESC
                          OFFSET 0 LIMIT @takeCount";

        return await GetMatchesByPredicateAsync(
            predicate,
            new() { { "@playerId", id }, { "@takeCount", takeCount } },
        cancellationToken);
    }

    public async Task<HeadToHeadOutputModel> GetHeadToHeadAsync(string firstPlayerId, string secondPlayerId, CancellationToken cancellationToken = default)
    {
        var headToHeadMathesQuery = @"SELECT *
                                FROM matches m
                                WHERE (m.FirstPlayer.PlayerId = @firstPlayerId AND m.SecondPlayer.PlayerId = @secondPlayerId)
                                   OR (m.FirstPlayer.PlayerId = @secondPlayerId AND m.SecondPlayer.PlayerId = @firstPlayerId)
                                ORDER BY m.DATE DESC";

        var headToHeadMatches = await _repo.GetItemsFromQueryAsync<Match>(
            headToHeadMathesQuery,
            new() { { "@firstPlayerId", firstPlayerId }, { "@secondPlayerId", secondPlayerId } },
            cancellationToken);

        var tournamentsInfoQuery = @"SELECT VALUE {Id: t.id, Category: t.Category, Surface: t.Surface}
                                    FROM tournaments t
                                    WHERE t.id IN (@tournamentsId)";

        var tournamentsInfo = (await _repo.GetItemsFromQueryAsync<Tournament, TournamentBasicOutputModel>(
            tournamentsInfoQuery,
            new() { { "@tournamentsId", string.Join(",", headToHeadMatches.Select(m => m.TournamentId).Distinct()) } },
            cancellationToken))
            .ToDictionary(model => model.Id);


        return new HeadToHeadOutputModel
        {
            RecentMatches = _mapper.Map<IEnumerable<MatchOutputModel>>(headToHeadMatches),
            FirstPlayerGrandSlamWins = headToHeadMatches.Count(match =>
                match.WinnerId == firstPlayerId && tournamentsInfo[match.TournamentId].Category == "Grand Slam"),
            SecondPlayerGrandSlamWins = headToHeadMatches.Count(match =>
                match.WinnerId == secondPlayerId && tournamentsInfo[match.TournamentId].Category == "Grand Slam"),
            FirstPlayerWins = headToHeadMatches.GroupBy(match => tournamentsInfo[match.TournamentId].Surface)
                                               .ToDictionary(g => g.Key, g => g.Count(m => m.WinnerId == firstPlayerId)),
            SecondPlayerWins = headToHeadMatches.GroupBy(match => tournamentsInfo[match.TournamentId].Surface)
                                                .ToDictionary(g => g.Key, g => g.Count(m => m.WinnerId == secondPlayerId))
        };
    }

    public async Task<string> InsertPlayerAsync(PlayerInputModel inputModel, CancellationToken cancellationToken = default)
    {
        var pictureUrl = await _imageStoreService.StoreImageAsync(inputModel.Picture, _configuration["ImageStore:PlayerImages"], cancellationToken);
        var standingPictureUrl = await _imageStoreService.StoreImageAsync(inputModel.StandingPicture, _configuration["ImageStore:PlayerImages"], cancellationToken);

        var player = _mapper.Map<Player>(inputModel, options => options.AfterMap((_, b) =>
        {
            b.PictureUrl = pictureUrl;
            b.StandingPictureUrl = standingPictureUrl;
        }));

        return await _repo.InsertAsync<Player>(player, cancellationToken);
    }

    public async Task UpdatePlayerAsync(string id, PlayerInputModel inputModel, CancellationToken cancellationToken = default)
    {
        // TODO: it's better to check if base 64 codes for new and old images are the same and then upload the image

        var pictureUrl = await _imageStoreService.StoreImageAsync(inputModel.Picture, _configuration["ImageSore:PlayerImages"], cancellationToken);
        var standingPictureUrl = await _imageStoreService.StoreImageAsync(inputModel.StandingPicture, _configuration["ImageSore:PlayerImages"], cancellationToken);

        var player = _mapper.Map<Player>(inputModel, options => options.AfterMap((_, b) =>
        {
            b.PictureUrl = pictureUrl;
            b.StandingPictureUrl = standingPictureUrl;
        }));

        await _repo.UpdateAsync<Player>(id, player, cancellationToken);
    }

    public async Task RemovePlayerAsync(string id, CancellationToken cancellationToken = default)
        => await _repo.RemoveAsync<Player>(id, cancellationToken);

    private async Task<IEnumerable<MatchOutputModel>> GetMatchesByPredicateAsync(string predicate, Dictionary<string, object> queryParams, CancellationToken cancellationToken = default)
    {
        var query = $@"SELECT *
                       FROM matches m
                       {predicate}";

        var matches = await _repo.GetItemsFromQueryAsync<Match>(query, queryParams, cancellationToken: cancellationToken);

        return _mapper.Map<IEnumerable<MatchOutputModel>>(matches);
    }
}
