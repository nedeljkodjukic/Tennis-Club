using System.ComponentModel.DataAnnotations;
using TennisClub.Api.Models.Cosmos.SimpleObjects;

namespace TennisClub.Api.Models.Input;

public class PlayerInputModel
{
    [Required]
    public string FirstName { get; init; }

    [Required]
    public string LastName { get; init; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime BirthDate { get; init; }

    [Required]
    public Country Country { get; init; }

    [Required]
    public string Bio { get; init; }

    [Required]
    public string StrongHand { get; init; }

    [Required]
    public string RacketBrand { get; init; }

    [Required]
    public int Height { get; init; }

    [Required]
    public int Weight { get; init; }

    [Required]
    public string Picture { get; init; }

    [Required]
    public string StandingPicture { get; init; }

    [Required]
    public ICollection<SocialNetworkAccount> Accounts { get; init; }
}
