using AutoMapper;
using Microsoft.Azure.Cosmos;
using TennisClub.Api.Models.Cosmos.Containers;
using TennisClub.Api.Models.Cosmos.SimpleObjects;
using TennisClub.Api.Models.Input;

namespace TennisClub.Api.Services;

public class MatchService
{
    private readonly BaseCosmosRepository _repo;
    private readonly IMapper _mapper;

    public MatchService(BaseCosmosRepository repo, IMapper mapper)
        => (_repo, _mapper) = (repo, mapper);

    public async Task<string> InsertAsync(MatchInputModel inputModel, CancellationToken cancellationToken = default)
    {
        var tournamentNameQuery = @"SELECT VALUE t.Name
                                    FROM tournaments t
                                    WHERE t.id = @tournamentId
                                    OFFSET 0 LIMIT 1";

        var tournamentName = await _repo.GetItemsFromQueryAsync<Tournament, string>(
            tournamentNameQuery,
            new() { { "@tournamentId", inputModel.TournamentId } },
            cancellationToken);

        var basicPlayersInfoQuery = @"SELECT VALUE { PlayerId: p.id, FirstName: p.FirstName, LastName: p.LastName}
                                      FROM players p
                                      WHERE p.id IN (@firstPlayerId, @secondPlayerId)";

        var playersInfo = await _repo.GetItemsFromQueryAsync<Player, PlayerBasicInfo>(basicPlayersInfoQuery,
            new() { { "@firstPlayerId", inputModel.FirstPlayerId }, { "@secondPlayerId", inputModel.SecondPlayerId } },
            cancellationToken);


        var match = _mapper.Map<Match>(inputModel, options =>
            options.AfterMap((_, m) =>
            {
                m.TournamentName = tournamentName.First();
                m.FirstPlayer = playersInfo.First();
                m.SecondPlayer = playersInfo.Last();
            })
        );

        var matchId = await _repo.InsertAsync<Match>(match, cancellationToken);

        await UpdatePlayerTitlesAndWinsAfterMatchAsync(match, cancellationToken);

        return matchId;
    }

    public async Task UpdateMatchAsync(string id, MatchUpdateInputModel inputModel, CancellationToken cancellationToken = default)
    {
        var match = _mapper.Map<Match>(inputModel);
        await _repo.UpdateAsync<Match>(id, match, cancellationToken);
    }

    public Task RemoveMatchAsync(string id, CancellationToken cancellationToken = default)
        => _repo.RemoveAsync<Match>(id, cancellationToken);


    private async Task UpdatePlayerTitlesAndWinsAfterMatchAsync(Match match, CancellationToken cancellationToken = default)
    {
        var winnerPatchOperations = new List<PatchOperation>
        {
            PatchOperation.Increment($"/{nameof(Player.TotalWins)}", 1)
        };

        if (match.Phase.ToLower() == "final")
            winnerPatchOperations.Add(PatchOperation.Increment($"/{nameof(Player.Titles)}", 1));

        await _repo.Containers[typeof(Player)].PatchItemAsync<Player>(id: match.WinnerId,
            partitionKey: new PartitionKey(match.WinnerId),
            patchOperations: winnerPatchOperations,
            cancellationToken: cancellationToken);

        var losserId = match.WinnerId == match.FirstPlayer.PlayerId
            ? match.SecondPlayer.PlayerId
            : match.FirstPlayer.PlayerId;

        await _repo.Containers[typeof(Player)].PatchItemAsync<Player>(id: losserId,
            partitionKey: new PartitionKey(losserId),
            patchOperations: new[] {
                PatchOperation.Increment($"/{nameof(Player.TotalLosses)}", 1)
            },
            cancellationToken: cancellationToken);
    }
}
