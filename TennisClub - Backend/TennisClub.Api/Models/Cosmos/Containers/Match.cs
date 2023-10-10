using TennisClub.Api.Models.Cosmos.SimpleObjects;

namespace TennisClub.Api.Models.Cosmos.Containers;

public class Match : BaseContainerItem
{
    public PlayerBasicInfo FirstPlayer { get; set; }

    public PlayerBasicInfo SecondPlayer { get; set; }

    public DateTime Date { get; set; }

    public int DurationInMinutes { get; set; } //in minutes

    public string WinnerId { get; set; }

    public string Phase { get; set; } //round

    public int Attendance { get; set; }

    public string CourtName { get; set; }

    public string TournamentId { get; set; }

    public string TournamentName { get; set; }

    public ICollection<Set> Sets { get; set; }
}
