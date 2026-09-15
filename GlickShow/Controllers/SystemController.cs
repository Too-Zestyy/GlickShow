using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace GlickShow.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class SystemController : ControllerBase
{
    private UserManager<AppUser> _userManager;

    public SystemController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("new")]
    public async Task<ActionResult<Glicko2System>> CreateNewSystem(
        AppDBContext db,
        [FromBody] CreateNewSystemParams parameters
    )
    {
        Duration duration;
        if (parameters.PeriodDuration == null)
        {
            duration = Duration.FromDays(7);
        }
        else
        {
            duration = parameters.PeriodDuration.ToDuration();
        }

        var system = new Glicko2System
        {
            Constant =
                parameters.Constant
                ?? GlickoCalc.Constants.DefaultSystemConstant,
            PeriodDuration = duration,
        };
        db.Systems.Add(system);

        await db.SaveChangesAsync();
        return Ok(new NewSystemConfirmation { Id = system.Id });
    }
}
