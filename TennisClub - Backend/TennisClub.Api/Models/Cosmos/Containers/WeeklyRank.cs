using TennisClub.Api.Models.Cosmos.SimpleObjects;

namespace TennisClub.Api.Models.Cosmos.Containers;

public class WeeklyRank : BaseContainerItem
{
    public int Year { get; set; }

    public int Week { get; set; }

    public string WeekDuration { get; set; }

    public ICollection<PlayerRankInfo> Ranks { get; set; }
}
