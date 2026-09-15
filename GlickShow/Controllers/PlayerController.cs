using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlickShow.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayerController : ControllerBase
{
    private UserManager<AppUser> _userManager;

    public PlayerController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet(Name = "Hello")]
    public ActionResult<string> GetHello()
    {
        return Ok("Hello From GlickShow!");
    }

    [HttpPost("new")]
    [Authorize]
    public async Task<IActionResult> AddNewPlayerToSystem(
        AppDBContext db,
        [FromBody] AddPlayerToSystemParameters parameters
    )
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }
        var userInDb = await _userManager.GetUserAsync(User);
        if (userInDb == null)
        {
            return Unauthorized();
        }

        if (
            !await db.Systems.Where(s => s.Id == parameters.SystemId).AnyAsync()
        )
        {
            return ValidationProblem("no system exists with requested id");
        }

        if (
            await db
                .Players.Where(p =>
                    p.AppUserId == userInDb.Id
                    && p.SystemId == parameters.SystemId
                )
                .AnyAsync()
        )
        {
            return ValidationProblem(
                "a player already exists for this user and system"
            );
        }

        await db.Players.AddAsync(
            new Glicko2Player(userInDb.Id, parameters.SystemId)
        );
        await db.SaveChangesAsync();
        return Ok();
    }

    // DB Model requires a linked user, so this testing endpoint is no longer viable without alternate classes for base glicko 2 parameters
    // [HttpGet("PlayMatch")]
    // [ProducesResponseType<Glicko2PlayerPair>(StatusCodes.Status200OK)]
    // public IActionResult PlayGlickoMatch([FromBody] Glicko2MatchParameters match)
    // {
    //     (double p1Nrating, double p1Ndeviation, double p1NVolatility) = GlickoCalc.Steps.UpdatePlayerFromMatches(
    //         match.PlayerOne.Rating, match.PlayerOne.Deviation, match.PlayerOne.Volatility,
    //         [match.PlayerTwo.Rating], [match.PlayerTwo.Deviation], [match.GameOutcome],
    //         GlickoCalc.Constants.DefaultSystemConstant, GlickoCalc.Constants.DefaultConvergenceTolerance
    //     );

    //     (double p2Nrating, double p2Ndeviation, double p2NVolatility) = GlickoCalc.Steps.UpdatePlayerFromMatches(
    //         match.PlayerTwo.Rating, match.PlayerTwo.Deviation, match.PlayerTwo.Volatility,
    //         [match.PlayerOne.Rating], [match.PlayerOne.Deviation], [1 - match.GameOutcome],
    //         GlickoCalc.Constants.DefaultSystemConstant, GlickoCalc.Constants.DefaultConvergenceTolerance
    //     );
    //     return Ok(new Glicko2PlayerPair(new Glicko2Player(p1Nrating, p1Ndeviation, p1NVolatility), new Glicko2Player(p2Nrating, p2Ndeviation, p2NVolatility)));
    // }

    [HttpGet("test-db")]
    public async Task<ActionResult<Glicko2System>> TestDb(AppDBContext db)
    {
        Random rand = new Random();
        db.Systems.Add(
            new Glicko2System { PeriodDuration = NodaTime.Period.FromDays(7) }
        );
        await db.SaveChangesAsync();
        var q = await db.Systems.OrderByDescending(s => s.Id).FirstAsync();

        return Ok(q);
    }
}
