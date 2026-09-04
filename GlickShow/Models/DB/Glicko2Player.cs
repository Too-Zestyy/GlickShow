using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Glicko2Player
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    
    public double Rating { get; set; }
    public double Deviation { get; set; }
    public double Volatility { get; set; }

    public Glicko2Player(double rating, double deviation, double volatility)
    {
        Rating = rating;
        Deviation = deviation;
        Volatility = volatility;
    }

    public Glicko2Player()
    {
        Rating = GlickoCalc.Constants.DefaultPlayerRating;
        Deviation = GlickoCalc.Constants.DefaultPlayerDeviation;
        Volatility = GlickoCalc.Constants.DefaultPlayerVolatility;
    }
}