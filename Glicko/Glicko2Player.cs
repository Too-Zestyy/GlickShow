

public class Glicko2Player
{
    public const double DefaultRating = 0, DefaultDeviation = 350/173.7178, DefaultVolatility = 0.06;

    double Rating { get; }
    double Deviation { get; }
    double Volatility { get; }

    public Glicko2Player(double rating, double deviation, double volatility)
    {
        Rating = rating;
        Deviation = deviation;
        Volatility = volatility;
    }

    public Glicko2Player()
    {
        Rating = DefaultRating;
        Deviation = DefaultDeviation;
        Volatility = DefaultVolatility;
    }
}