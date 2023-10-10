using TennisClub.Api.Models.Cosmos.SimpleObjects;
using TennisClub.Api.Models.Input;

namespace TennisClub.Api.Models.Output;

public class TournamentOutputModel
{
    public string Id { get; init; }
    public string Name { get; init; }

    public string City { get; init; }

    public Country Country { get; init; }

    public string Surface { get; init; }

    public string Category { get; init; }

    public string Logo { get; set; }

    public string AtpPoints { get; init; }

    public string TournamentInfo { get; init; }

    public ICollection<CourtInputOutputModel> Courts { get; init; }
}
