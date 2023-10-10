using TennisClub.Api.Models.Cosmos.SimpleObjects;

namespace TennisClub.Api.Models.Output;

public class MatchOutputModel
{
    public string FirstPlayerFullName { get; init; }

    public string SecondPlayerFullName { get; init; }

    public DateTime Date { get; init; }

    public int DurationInMinutes { get; init; }

    public string Phase { get; init; }

    public int Attendance { get; init; }

    public string CourtName { get; init; }

    public string TournamentName { get; init; }

    public ICollection<Set> Sets { get; init; }

    public string Result
        => $"{Sets.Count(init => init.FirstPlayerScore > init.SecondPlayerScore)}:{Sets.Count(init => init.FirstPlayerScore < init.SecondPlayerScore)}";
}
