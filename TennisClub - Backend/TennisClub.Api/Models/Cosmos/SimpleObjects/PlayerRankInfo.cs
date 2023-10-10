namespace TennisClub.Api.Models.Cosmos.SimpleObjects;

public class PlayerRankInfo
{
    public string PlayerId { get; set; }

    public int RankNumber { get; set; }

    public int PreviousRankNumber { get; set; }

    public int Points { get; set; }
}
