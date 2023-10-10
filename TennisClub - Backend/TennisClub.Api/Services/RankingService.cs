using AutoMapper;
using Microsoft.Azure.Cosmos;
using System.Globalization;
using TennisClub.Api.Models.Cosmos.Containers;
using TennisClub.Api.Models.Cosmos.SimpleObjects;
using TennisClub.Api.Models.Input;
using TennisClub.Api.Models.Output;

namespace TennisClub.Api.Services;

public class RankingService
{
    private readonly BaseCosmosRepository _repo;
    private readonly IMapper _mapper;
    private readonly ImageStoreService _imageStoreService;

    public RankingService(BaseCosmosRepository repo, IMapper mapper, ImageStoreService imageStoreService)
        => (_repo, _mapper, _imageStoreService) = (repo, mapper, imageStoreService);

    public async Task<Dictionary<int, List<RankBasicOutputModel>>> GetRanksForPlayerByYearAsync(string playerId, CancellationToken cancellationToken = default)
    {
        var ranksByYearQuery = @"SELECT wr.Year, wr.Week, wr.WeekDuration, r.RankNumber, r.Points
                                 FROM weeklyRanks wr
                                 JOIN r IN wr.Ranks
                                 WHERE r.PlayerId = @playerId";

        var ranks = await _repo.GetItemsFromQueryAsync<WeeklyRank, RankBasicOutputModel>(
            ranksByYearQuery,
            new() { { "@playerId", playerId } },
            cancellationToken);

        return ranks.GroupBy(r => r.Year)
                    .ToDictionary(g => g.Key, g => g.ToList());
    }

    public async Task InsertRanksAsync(int year, int week, IEnumerable<RankInputModel> ranks, CancellationToken cancellationToken = default)
    {
        var weekDuration = ISOWeek.ToDateTime(year, week, DayOfWeek.Monday).ToString("dd.MM.yyyy")
            + " - "
            + ISOWeek.ToDateTime(year, week, DayOfWeek.Sunday).ToString("dd.MM.yyyy");

        var rank = new WeeklyRank
        {
            Year = year,
            Week = week,
            WeekDuration = weekDuration,
            Ranks = ranks.Select(r => new PlayerRankInfo
            {
                PlayerId = r.PlayerId,
                Points = r.Points,
                PreviousRankNumber = r.PreviousRankNumber, // get previous rank from db
                RankNumber = r.RankNumber,
            }).ToList()
        };

        await _repo.InsertAsync<WeeklyRank>(rank, cancellationToken);
        /*
        Expression<Func<Rank, bool>> filter;

        if (week == 1)
        {
            filter = rank => rank.Year == year - 1 && rank.Week == t;
        }
        else
        {
            filter = rank => rank.Year == year && rank.Week == week - 1;
        }

        var previousRanks = (await ranksCollection.AsQueryable()
            .Where(filter)
            .Select(rank => new { rank.PlayerId, rank.RankNumber })
            .ToListAsync())
            .ToDictionary(rank => rank.PlayerId, rank => rank.RankNumber);
        */
    }

    private record PlayerRankRecord(string PlayerId, int Points, int PreviousRankNumber, int RankNumber);

    public async Task<IEnumerable<RankOutputModel>> GetTopRatedPlayersAsync(int year, int week, int topCount, CancellationToken cancellationToken = default)
    {
        if (year == 0 || week == 0)
        {
            var currentDate = DateTime.Now.AddDays(-7);
            year = ISOWeek.GetYear(currentDate);
            week = ISOWeek.GetWeekOfYear(currentDate);
        }

        var topRatedPlayersQuery = @"SELECT r.PlayerId, r.Points, r.PreviousRankNumber, r.RankNumber
                                     FROM weeklyRanks wr
                                     JOIN r IN wr.Ranks
                                     WHERE wr.Year = @year AND wr.Week = @week";
        //ORDER BY r.RankNumber
        //OFFSET 0 LIMIT @count";

        var ranks = await _repo.GetItemsFromQueryAsync<WeeklyRank, PlayerRankRecord>(
            topRatedPlayersQuery,
            new() { { "@year", year }, { "@week", week } }, //{ "@count", topCount } },
            cancellationToken);

        ranks = ranks.OrderBy(r => r.RankNumber).Take(topCount);

        List<(string, PartitionKey)> itemsToFind = ranks.DistinctBy(r => r.PlayerId).Select(r => (r.PlayerId, new PartitionKey(r.PlayerId))).ToList();

        var players = await _repo.Containers[typeof(Player)].ReadManyItemsAsync<Player>(items: itemsToFind, cancellationToken: cancellationToken);

        return ranks.Select(async r =>
        {
            var player = players.Single(p => p.Id == r.PlayerId);
            var picture = await _imageStoreService.GetBase64StringImageFromUrlAsync(player.PictureUrl, cancellationToken);
            return new RankOutputModel
            {
                Country = player.Country,
                FirstName = player.FirstName,
                LastName = player.LastName,
                Picture = picture,
                Rank = r.RankNumber,
                PreviousRank = r.PreviousRankNumber,
                Points = r.Points,
                PlayerId = r.PlayerId,
            };
        }).Select(r => r.Result);
    }
}
