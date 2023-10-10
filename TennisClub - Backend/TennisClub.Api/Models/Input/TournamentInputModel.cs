using System.ComponentModel.DataAnnotations;
using TennisClub.Api.Models.Cosmos.SimpleObjects;

namespace TennisClub.Api.Models.Input;

public class TournamentInputModel
{
    [Required]
    public string Name { get; init; }


    [Required]
    public string City { get; init; }


    [Required]
    public Country Country { get; init; }


    [Required]
    public string Surface { get; init; }


    [Required]
    public string Category { get; init; }


    [Required]
    public string AtpPoints { get; init; }


    [Required]
    public string Logo { get; init; }


    [Required]
    public string TournamentInfo { get; init; }


    [Required]
    public IEnumerable<CourtInputOutputModel> Courts { get; init; }
}
