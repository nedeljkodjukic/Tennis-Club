namespace TennisClub.Api.Models.Output;

public class HeadToHeadOutputModel
{
    public IEnumerable<MatchOutputModel> RecentMatches { get; init; }

    public Dictionary<string, int> FirstPlayerWins { get; init; }

    public Dictionary<string, int> SecondPlayerWins { get; init; }

    public int FirstPlayerGrandSlamWins { get; init; }

    public int SecondPlayerGrandSlamWins { get; init; }

    public int FirstPlayerTotalWins => FirstPlayerWins.Values.Sum();

    public int SecondPlayerTotalWins => SecondPlayerWins.Values.Sum();

}
