using TennisClub.Api.Models.Cosmos.SimpleObjects;

namespace TennisClub.Api.Models.Output;

public class RankOutputModel
{
    public string PlayerId { get; init; }

    public string Picture { get; init; }

    public Country Country { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public int Points { get; init; }

    public int Rank { get; init; }

    public int PreviousRank { get; init; }
}
