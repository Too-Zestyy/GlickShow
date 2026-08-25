using Microsoft.AspNetCore.Mvc;

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
}
