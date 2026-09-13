using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace GlickShow.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    [HttpPost()]
    public async Task<ActionResult<Glicko2System>> TestDb(AppDBContext db)
    {
        Random rand = new Random();
        db.Systems.Add(new Glicko2System {PeriodDuration = NodaTime.Period.FromDays(7)});
        await db.SaveChangesAsync();
        var q = await db.Systems.OrderByDescending(s => s.Id).FirstAsync();

        return Ok(q);
    }
}
