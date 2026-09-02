

public class Glicko2Player
{

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