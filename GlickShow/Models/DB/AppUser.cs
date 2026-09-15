using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser
{
    public ICollection<Glicko2Player> Players { get; } =
        new List<Glicko2Player>();
}
