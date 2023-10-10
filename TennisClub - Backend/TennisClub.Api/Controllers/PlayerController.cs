using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TennisClub.Api.Models.Input;
using TennisClub.Api.Models.Output;
using TennisClub.Api.Services;

namespace TennisClub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayerController : ControllerBase
{
    private readonly PlayerService _playerService;

    public PlayerController(PlayerService playerService)
        => _playerService = playerService;

    [AllowAnonymous]
    [HttpGet]
    [Route("get/{id}")]
    public async Task<ActionResult<PlayerOutputModel>> Get([FromRoute] string id, CancellationToken cancellationToken = default)
    {
        var result = await _playerService.GetAsync(id, cancellationToken);

        return Ok(result);
    }

    [AllowAnonymous]
    //[Authorize(Roles = "User, Admin")]
    [HttpGet]
    [Route("get_all_basic")]
    public async Task<ActionResult<IEnumerable<PlayerBasicOutputModel>>> GetAllBasic(CancellationToken cancellationToken = default)
    {
        var result = await _playerService.GetAllPlayersBasicInfoAsync(cancellationToken);

        return Ok(result);
    }

    [AllowAnonymous]
    //[Authorize(Roles = "User")]
    [HttpGet]
    [Route("get_head_to_head")]
    public async Task<ActionResult<HeadToHeadOutputModel>> GetHeadToHead([FromQuery] string firstPlayerId, [FromQuery] string secondPlayerId, CancellationToken cancellationToken = default)
    {
        var result = await _playerService.GetHeadToHeadAsync(firstPlayerId, secondPlayerId, cancellationToken);

        return Ok(result);
    }

    [AllowAnonymous]
    //[Authorize(Roles = "User")]
    [HttpGet]
    [Route("get_recent_matches/{id}")]
    public async Task<ActionResult<HeadToHeadOutputModel>> GetRecentMatches([FromRoute] string id, CancellationToken cancellationToken = default)
    {
        var result = await _playerService.GetRecentMachesAsync(id, 5, cancellationToken);

        return Ok(result);
    }

    [AllowAnonymous]
    //[Authorize(Roles = "User")]
    [HttpGet]
    [Route("get_title_matches/{id}")]
    public async Task<ActionResult<HeadToHeadOutputModel>> GetTitleMatches([FromRoute] string id, CancellationToken cancellationToken = default)
    {
        var result = await _playerService.GetFinalMatchesAsync(id, cancellationToken);

        return Ok(result);
    }

    [AllowAnonymous]
    //[Authorize(Roles = "Admin, User")]
    [HttpGet]
    [Route("get_all")]
    public async Task<ActionResult<IEnumerable<PlayerOutputModel>>> GetAll(CancellationToken cancellationToken = default)
    {
        var result = await _playerService.GetAllAsync(cancellationToken);

        return Ok(result);
    }

    [AllowAnonymous]
    //[Authorize(Roles = "Admin")]
    [HttpPost]
    [Route("insert")]
    public async Task<ActionResult<string>> Insert([FromBody] PlayerInputModel input, CancellationToken cancellationToken = default)
    {
        var result = await _playerService.InsertPlayerAsync(input, cancellationToken);

        return Ok(result);
    }

    [AllowAnonymous]
    //[Authorize(Roles = "Admin")]
    [HttpPost]
    [Route("update/{id}")]
    public async Task<ActionResult<string>> Update([FromRoute] string id, [FromBody] PlayerInputModel input, CancellationToken cancellationToken = default)
    {
        await _playerService.UpdatePlayerAsync(id, input, cancellationToken);

        return Ok();
    }

    [AllowAnonymous]
    //[Authorize(Roles = "Admin")]
    [HttpDelete]
    [Route("remove/{id}")]
    public async Task<ActionResult> Remove([FromRoute] string id, CancellationToken cancellationToken = default)
    {
        await _playerService.RemovePlayerAsync(id, cancellationToken);

        return Ok();
    }
}
