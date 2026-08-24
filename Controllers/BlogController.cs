using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class BlogController : ControllerBase
{

    [HttpGet(Name = "SayHello")]
    
    public IActionResult Get()
    {
        return Ok("Hello World!");
    }
}
