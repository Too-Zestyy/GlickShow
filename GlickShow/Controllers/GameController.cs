using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GlickShow.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private UserManager<AppUser> _userManager;

    public GameController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
}
