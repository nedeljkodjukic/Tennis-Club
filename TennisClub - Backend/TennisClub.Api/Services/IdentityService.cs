using Microsoft.AspNetCore.Identity;
using TennisClub.Api.Enums;
using TennisClub.Api.Extensions;
using TennisClub.Api.Models.Cosmos.Containers;
using TennisClub.Api.Models.Identity;

namespace TennisClub.Api.Services;

public class IdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<Mobsites.Cosmos.Identity.IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public IdentityService(UserManager<ApplicationUser> userManager,
        RoleManager<Mobsites.Cosmos.Identity.IdentityRole> roleManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<TokenOutputModel> LoginUserAsync(LoginInputModel input)
    {
        var user = await _userManager.FindByEmailAsync(input.Email) ?? throw new Exception($"User with email: {input.Email} does not exist");

        var result = await _signInManager.PasswordSignInAsync(user.UserName, input.Password, false, false);

        if (result.Succeeded)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var key = _configuration["Authorization:SecretKey"];

            return JwtAuthExtensions.GenerateToken(user, roles, key, 7);
        }

        throw new Exception($"Login error: {result}");
    }

    public async Task<ApplicationUser> RegisterUserAsync(RegisterInputModel input)
    {
        var user = new ApplicationUser(input.UserName)
        {
            Email = input.Email,
            FirstName = input.FirstName,
            LastName = input.LastName
        };

        var result = await _userManager.CreateAsync(user, input.Password);

        if (!result.Succeeded)
        {
            throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(x => x.Description)));
        };

        return user;
    }

    public async Task RegisterUserToRoleAsync(ApplicationUser user, Role role)
    {
        await CreateRolesIfDoNotExists();

        await _userManager.AddToRoleAsync(user, role.ToString());
    }

    private async Task CreateRolesIfDoNotExists()
    {
        if (!await _roleManager.RoleExistsAsync(Role.Admin.ToString()))
            await _roleManager.CreateAsync(new Mobsites.Cosmos.Identity.IdentityRole { Name = Role.Admin.ToString() });
        if (!await _roleManager.RoleExistsAsync(Role.User.ToString()))
            await _roleManager.CreateAsync(new Mobsites.Cosmos.Identity.IdentityRole { Name = Role.User.ToString() });
    }
}
