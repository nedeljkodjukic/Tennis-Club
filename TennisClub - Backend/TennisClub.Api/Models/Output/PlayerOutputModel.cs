using TennisClub.Api.Models.Cosmos.SimpleObjects;

namespace TennisClub.Api.Models.Output;

public class PlayerOutputModel
{
    public string Id { get; init; }
    public string FirstName { get; init; }

    public string LastName { get; init; }

    public DateTime BirthDate { get; init; }

    public Country Country { get; init; }

    public string Bio { get; init; }

    public string StrongHand { get; init; }

    public string RacketBrand { get; init; }

    public int Height { get; init; }

    public int Titles { get; init; }

    public int Weight { get; init; }

    public int Rank { get; set; }

    public string Picture { get; set; }

    public string StandingPicture { get; set; }

    public string Instagram { get; init; }

    public string Twitter { get; init; }

    public int TotalWins { get; init; }

    public int TotalLosses { get; init; }
}
