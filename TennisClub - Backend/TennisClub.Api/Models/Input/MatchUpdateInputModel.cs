using System.ComponentModel.DataAnnotations;
using TennisClub.Api.Models.Cosmos.SimpleObjects;

namespace TennisClub.Api.Models.Input;

public class MatchUpdateInputModel
{
    [Required]
    public DateTime Date { get; init; }

    [Required]
    public string Phase { get; init; }

    [Required]
    public int Attendance { get; init; }

    [Required]
    public string CourtName { get; init; }

    [Required]
    public ICollection<Set> Sets { get; init; }
}
