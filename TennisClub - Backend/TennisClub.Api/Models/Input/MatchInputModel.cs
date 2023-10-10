using System.ComponentModel.DataAnnotations;
using TennisClub.Api.Models.Cosmos.SimpleObjects;

namespace TennisClub.Api.Models.Input;

public class MatchInputModel
{
    [Required]
    public string FirstPlayerId { get; init; }

    [Required]
    public string SecondPlayerId { get; init; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime Date { get; init; }

    [Required]
    public string Phase { get; init; }

    [Required]
    public int Attendance { get; init; }

    [Required]
    public string CourtName { get; init; }

    [Required]
    public string TournamentId { get; init; }

    [Required]
    public ICollection<Set> Sets { get; init; }
}
