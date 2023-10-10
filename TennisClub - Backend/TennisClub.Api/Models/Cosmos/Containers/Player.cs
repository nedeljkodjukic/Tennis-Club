using TennisClub.Api.Models.Cosmos.SimpleObjects;

namespace TennisClub.Api.Models.Cosmos.Containers;

public class Player : BaseContainerItem
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime BirthDate { get; set; }

    public Country Country { get; set; }

    public string Bio { get; set; }

    public string StrongHand { get; set; }

    public string RacketBrand { get; set; }

    public int Height { get; set; }

    public int Titles { get; set; }

    public int TotalWins { get; set; }

    public int TotalLosses { get; set; }

    public int Weight { get; set; }

    public ICollection<SocialNetworkAccount> Accounts { get; set; }

    public string PictureUrl { get; set; }

    public string StandingPictureUrl { get; set; }
}
