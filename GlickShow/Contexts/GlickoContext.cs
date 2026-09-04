using Microsoft.EntityFrameworkCore;

public class GlickoContext(DbContextOptions<GlickoContext> options) : DbContext(options)
{
    public DbSet<Glicko2System> Systems { get; set; }
}