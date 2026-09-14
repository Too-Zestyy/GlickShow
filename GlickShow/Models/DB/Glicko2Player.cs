using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Glicko2Player
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int SystemId { get; set; }
    public Glicko2System System { get; set; } = null!;

    public string AppUserId { get; set; }
    public AppUser AppUser { get; } = null!;

    
    public double Rating { get; set; }
    public double Deviation { get; set; }
    public double Volatility { get; set; }


    public Glicko2Player(string appUserId, int systemId)
    {
        AppUserId = appUserId;
        SystemId = systemId;
        Rating = GlickoCalc.Constants.DefaultPlayerRating;
        Deviation = GlickoCalc.Constants.DefaultPlayerDeviation;
        Volatility = GlickoCalc.Constants.DefaultPlayerVolatility;
    }

    public Glicko2Player(string appUserId, int systemId, double rating, double deviation, double volatility) 
    : this(appUserId, systemId)
    {
        Rating = rating;
        Deviation = deviation;
        Volatility = volatility;
    }

}