using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TennisClub.Api.Enums;
using TennisClub.Api.Models.Identity;
using TennisClub.Api.Services;

namespace TennisClub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IdentityController : ControllerBase
{
    private readonly IdentityService _identityService;

    public IdentityController(IdentityService identityService)
        => _identityService = identityService;

    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<TokenOutputModel>> Login([FromBody] LoginInputModel input, CancellationToken cancellationToken = default)
    {
        var token = await this._identityService.LoginUserAsync(input);

        this.Response.Cookies.Delete(".AspNetCore.Identity.Application");

        return Ok(token);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("register/{role}")]
    public async Task<ActionResult<int>> Register([FromRoute] Role role, [FromBody] RegisterInputModel input, CancellationToken cancellationToken = default)
    {
        var user = await this._identityService.RegisterUserAsync(input);

        await this._identityService.RegisterUserToRoleAsync(user, role);

        return Ok(user.Id);
    }
}
