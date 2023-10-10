using Mobsites.Cosmos.Identity;

namespace TennisClub.Api.Models.Cosmos.Containers;

public class ApplicationUser : IdentityUser
{
    public ApplicationUser()
    {
    }

    public ApplicationUser(string userName)
    {
        UserName = userName;
    }

    public string FirstName { get; set; }

    public string LastName { get; set; }
}
