namespace TennisClub.Api.Models.Identity;

public class TokenOutputModel
{
    public string AccessToken { get; init; }
    public DateTime Expires { get; init; }
    public string Name { get; init; }
    public string Surname { get; init; }
}
