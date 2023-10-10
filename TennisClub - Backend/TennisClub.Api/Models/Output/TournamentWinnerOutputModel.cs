namespace TennisClub.Api.Models.Output;

public class TournamentWinnerOutputModel
{
    public PlayerBasicOutputModel Player { get; init; }

    public int Year { get; init; }
}
