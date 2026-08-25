public class Glicko2System
{
    public const double GameLoss = 0, GameDraw = 0.5, GameWin = 1;
    public const double DefaultConvergenceTolerance = 0.000001;
    public const double LowConstant = 0.3, DefaultConstant = 0.5, HighConstant = 1.2;



    /// <summary>
    /// Calculates <c>g(φ)</c>, which is used as a component of game variance within a period.
    /// </summary>
    /// <param name="deviation">The deviation of the opponent player to calculate the component of subsequent variance for.</param>
    /// <returns><c>g(φ)</c></returns>
    private double step3g(double deviation)
    {
        double deviationSquared = Math.Pow(deviation, 2);
        return 1 / Math.Sqrt(1 + 3 * deviationSquared / Math.Pow(Math.PI, 2));
    }
    

    /// <summary>
    /// Calculates <c>E(µ, µj, φj)</c>, which is used as a component of game variance within a period.
    /// </summary>
    /// <param name="rating">The rating of the current player.</param>
    /// <param name="opponentRating">The rating of the player's opponent.</param>
    /// <param name="opponentDeviation">The opponent's rating deviation.</param>
    /// <returns><c>E(µ, µj, φj)</c></returns>
    private double step3E(double rating, double opponentRating, double opponentDeviation)
    {
        return 1 / (1 + Math.Exp(-step3g(opponentDeviation) * (rating - opponentRating)));
    }

    /// <summary>
    /// calculateVarianceFromGameOutcomes calculates <c>𝒱</c>, which is a player's variance within a period solely from game outcomes. 
    /// Equivalent to the entirety of step 3.
    /// </summary>
    /// <param name="playerRating">The rating of the player to calculate the variance of based on matches against all opponents.</param>
    /// <param name="opponentRatings">An array of all opponent ratings.</param>
    /// <param name="opponentDeviations">An array of all opponent rating deviations.</param>
    /// <returns>The deviation of the player when a match has been played once for each opponent rating and deviation.</returns>
    /// <exception cref="ArgumentException">If the number of opponent ratings and deviations do not match.</exception>
    private double calculateVarianceFromGameOutcomes(double playerRating, double[] opponentRatings, double[] opponentDeviations)
    {
        if (opponentRatings.Length != opponentDeviations.Length)
        {
            throw new ArgumentException("The length of `opponentRatings` and `opponentDeviations` must be equal.");
        }

        double sum = 0;

        for (int i = 0; i < opponentRatings.Length; i++)
        {
            double curMatchE = step3E(playerRating, opponentRatings[i], opponentDeviations[i]);
            sum += Math.Pow(step3g(opponentDeviations[i]), 2) * curMatchE * (1 - curMatchE);
        }

        return 1 / sum;
    }
}