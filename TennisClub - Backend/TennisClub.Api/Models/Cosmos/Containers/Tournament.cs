using TennisClub.Api.Models.Cosmos.SimpleObjects;

namespace TennisClub.Api.Models.Cosmos.Containers;

public class Tournament : BaseContainerItem
{
    public string Name { get; set; }

    public string City { get; set; }

    public Country Country { get; set; }

    public string Surface { get; set; }

    public string Category { get; set; }

    public string AtpPoints { get; set; }

    public string LogoUrl { get; set; }

    public string TournamentInfo { get; set; }

    public ICollection<Court> Courts { get; set; }
}
