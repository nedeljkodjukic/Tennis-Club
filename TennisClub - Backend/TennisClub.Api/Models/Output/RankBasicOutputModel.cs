namespace TennisClub.Api.Models.Output;

public class RankBasicOutputModel
{
    public int Year { get; set; }

    public int Week { get; init; }

    public string WeekDuration { get; init; }

    public int RankNumber { get; init; }

    public int Points { get; init; }
}
