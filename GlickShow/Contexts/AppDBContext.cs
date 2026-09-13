using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDBContext : IdentityDbContext<AppUser>
{
    public AppDBContext(DbContextOptions<AppDBContext> options) :
        base(options)
    { }

    public DbSet<Glicko2System> Systems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Initialise Identity model
        base.OnModelCreating(builder);

        ////////////////////////////////////
        // Glicko 2 System Default Values //
        ////////////////////////////////////
        builder.Entity<Glicko2System>().Property(s => s.Epoch).HasDefaultValueSql("current_timestamp");
        builder.Entity<Glicko2System>().Property(s => s.Constant).HasDefaultValue(GlickoCalc.Constants.DefaultSystemConstant);

        ///////////////////////////////
        // New player ranking values //
        ///////////////////////////////
        builder.Entity<Glicko2Player>().Property(p => p.Rating).HasDefaultValue(GlickoCalc.Constants.DefaultPlayerRating);
        builder.Entity<Glicko2Player>().Property(p => p.Deviation).HasDefaultValue(GlickoCalc.Constants.DefaultPlayerDeviation);
        builder.Entity<Glicko2Player>().Property(p => p.Volatility).HasDefaultValue(GlickoCalc.Constants.DefaultPlayerVolatility);
    }
}