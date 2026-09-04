using Microsoft.EntityFrameworkCore;

public class GlickoContext(DbContextOptions<GlickoContext> options) : DbContext(options)
{
    public DbSet<Glicko2System> Systems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ////////////////////////////////////
        // Glicko 2 System Default Values //
        ////////////////////////////////////
        modelBuilder.Entity<Glicko2System>().Property(s => s.Epoch).HasDefaultValueSql("current_timestamp");
        modelBuilder.Entity<Glicko2System>().Property(s => s.Constant).HasDefaultValue(GlickoCalc.Constants.DefaultSystemConstant);

        ///////////////////////////////
        // New player ranking values //
        ///////////////////////////////
        modelBuilder.Entity<Glicko2Player>().Property(p => p.Rating).HasDefaultValue(GlickoCalc.Constants.DefaultPlayerRating);
        modelBuilder.Entity<Glicko2Player>().Property(p => p.Deviation).HasDefaultValue(GlickoCalc.Constants.DefaultPlayerDeviation);
        modelBuilder.Entity<Glicko2Player>().Property(p => p.Volatility).HasDefaultValue(GlickoCalc.Constants.DefaultPlayerVolatility);
    }
}