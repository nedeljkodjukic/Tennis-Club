using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TennisClub.Api.Models.Input;
using TennisClub.Api.Models.Output;
using TennisClub.Api.Services;

namespace TennisClub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TournamentController : ControllerBase
{
    private readonly TournamentService _tournamentService;

    public TournamentController(TournamentService tournamentService)
        => _tournamentService = tournamentService;

    [AllowAnonymous]
    //[Authorize(Roles = "Admin, User")]
    [HttpGet]
    [Route("get/{id}")]
    public async Task<ActionResult<TournamentOutputModel>> Get([FromRoute] string id, CancellationToken cancellationToken = default)
    {
        var result = await _tournamentService.GetAsync(id, cancellationToken);

        return Ok(result);
    }

    [AllowAnonymous]
    //[Authorize(Roles = "Admin, User")]
    [HttpGet]
    [Route("get_all")]
    public async Task<ActionResult<TournamentOutputModel>> GetAll(CancellationToken cancellationToken = default)
    {
        var result = await _tournamentService.GetAll(cancellationToken);

        return Ok(result);
    }

    [AllowAnonymous]
    //[Authorize(Roles = "Admin, User")]
    [HttpGet]
    [Route("get_all_basic")]
    public async Task<ActionResult<IEnumerable<TournamentBasicOutputModel>>> GetAllBasic(CancellationToken cancellationToken = default)
    {
        var result = await _tournamentService.GetAllTournamentsBasicInfoAsync(cancellationToken);

        return Ok(result);
    }

    [AllowAnonymous]
    //[Authorize(Roles = "User")]
    [HttpGet]
    [Route("get_past_winners/{tournamentId}")]
    public async Task<ActionResult<IEnumerable<TournamentWinnerOutputModel>>> GetPastWinners([FromRoute] string tournamentId, CancellationToken cancellationToken = default)
    {
        var result = await _tournamentService.GetPastWinnersAsync(tournamentId, cancellationToken);

        return Ok(result);
    }

    [AllowAnonymous]
    //[Authorize(Roles = "User")]
    [HttpGet]
    [Route("get_records/{tournamentId}")]
    public async Task<ActionResult<IEnumerable<PlayerBasicOutputModel>>> GetTournamentRecord([FromRoute] string tournamentId, CancellationToken cancellationToken = default)
    {
        var result = await _tournamentService.GetTournamentRecordAsync(tournamentId, cancellationToken);

        return Ok(result);
    }

    [AllowAnonymous]
    //[Authorize(Roles = "User, Admin")]
    [HttpGet]
    [Route("get_matches/{tournamentId}/{year}")]
    public async Task<ActionResult<MatchOutputModel>> GetMatches([FromRoute] string tournamentId, [FromRoute] int year, CancellationToken cancellationToken = default)
    {
        var result = await _tournamentService.GetMatchesByTournamentAndYearAsync(tournamentId, year, cancellationToken);

        return Ok(result);
    }

    [AllowAnonymous]
    //[Authorize(Roles = "Admin")]
    [HttpPost]
    [Route("insert")]
    public async Task<ActionResult<string>> Insert([FromBody] TournamentInputModel input, CancellationToken cancellationToken = default)
    {
        var result = await _tournamentService.InsertTournamentAsync(input, cancellationToken);

        return Ok(result);
    }

    [AllowAnonymous]
    //[Authorize(Roles = "Admin")]
    [HttpPut]
    [Route("update/{id}")]
    public async Task<ActionResult> Update([FromRoute] string id, [FromBody] TournamentInputModel input, CancellationToken cancellationToken = default)
    {
        await _tournamentService.UpdateTournamentAsync(id, input, cancellationToken);

        return Ok();
    }

    [AllowAnonymous]
    //[Authorize(Roles = "Admin")]
    [HttpDelete]
    [Route("remove/{id}")]
    public async Task<ActionResult> Remove([FromRoute] string id, CancellationToken cancellationToken = default)
    {
        await _tournamentService.RemoveTournamentAsync(id, cancellationToken);

        return Ok();
    }
}
