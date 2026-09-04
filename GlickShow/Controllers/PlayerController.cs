using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace GlickShow.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayerController : ControllerBase
{
    

    [HttpGet(Name = "Hello")]
    public ActionResult<string> GetHello()
    {
        
        return Ok("Hello From GlickShow!");
    }

    [HttpGet("PlayMatch")]
    [ProducesResponseType<Glicko2PlayerPair>(StatusCodes.Status200OK)]
    public IActionResult PlayGlickoMatch([FromBody] Glicko2MatchParameters match)
    {
        (double p1Nrating, double p1Ndeviation, double p1NVolatility) = GlickoCalc.Steps.UpdatePlayerFromMatches(
            match.PlayerOne.Rating, match.PlayerOne.Deviation, match.PlayerOne.Volatility,
            [match.PlayerTwo.Rating], [match.PlayerTwo.Deviation], [match.GameOutcome],
            GlickoCalc.Constants.DefaultSystemConstant, GlickoCalc.Constants.DefaultConvergenceTolerance
        );

        (double p2Nrating, double p2Ndeviation, double p2NVolatility) = GlickoCalc.Steps.UpdatePlayerFromMatches(
            match.PlayerTwo.Rating, match.PlayerTwo.Deviation, match.PlayerTwo.Volatility,
            [match.PlayerOne.Rating], [match.PlayerOne.Deviation], [1 - match.GameOutcome],
            GlickoCalc.Constants.DefaultSystemConstant, GlickoCalc.Constants.DefaultConvergenceTolerance
        );
        return Ok(new Glicko2PlayerPair(new Glicko2Player(p1Nrating, p1Ndeviation, p1NVolatility), new Glicko2Player(p2Nrating, p2Ndeviation, p2NVolatility)));
    }

    [HttpGet("test-db")]
    public async Task<ActionResult<Glicko2System>> TestDb(GlickoContext db)
    {
        Random rand = new Random();
        db.Systems.Add(new Glicko2System {PeriodDuration = NodaTime.Period.FromDays(7)});
        await db.SaveChangesAsync();
        var q = await db.Systems.OrderByDescending(s => s.ID).FirstAsync();

        return Ok(q);
    }
}
