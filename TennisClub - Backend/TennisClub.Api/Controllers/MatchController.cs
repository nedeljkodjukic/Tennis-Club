using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TennisClub.Api.Models.Input;
using TennisClub.Api.Services;

namespace TennisClub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatchController : ControllerBase
{
    private readonly MatchService _matchService;

    public MatchController(MatchService matchService)
        => _matchService = matchService;

    [AllowAnonymous]
    //[Authorize(Roles = "Admin")]
    [HttpPost]
    [Route("insert")]
    public async Task<ActionResult<string>> Insert([FromBody] MatchInputModel input, CancellationToken cancellationToken = default)
    {
        var result = await _matchService.InsertAsync(input, cancellationToken);

        return Ok(result);
    }

    [AllowAnonymous]
    //[Authorize(Roles = "Admin")]
    [HttpPut]
    [Route("update/{id}")]
    public async Task<ActionResult> Update([FromRoute] string id, [FromBody] MatchUpdateInputModel input, CancellationToken cancellationToken = default)
    {
        await _matchService.UpdateMatchAsync(id, input, cancellationToken);

        return Ok();
    }

    [AllowAnonymous]
    //[Authorize(Roles = "Admin")]
    [HttpDelete]
    [Route("remove/{id}")]
    public async Task<ActionResult> Remove([FromRoute] string id, CancellationToken cancellationToken = default)
    {
        await _matchService.RemoveMatchAsync(id, cancellationToken);

        return Ok();
    }
}
