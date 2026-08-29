namespace GlickoCalc
open System

module Steps =
    
    /// <summary>
    /// Calculates <c>g(φ)</c>, which is used as a component of game variance within a period.
    /// </summary>
    /// <param name="deviation">The deviation of the opponent player to calculate the component of subsequent variance for.</param>
    /// <returns><c>g(φ)</c></returns>
    let Three_g (deviation: float) = 
        let deviationSquared = deviation ** 2 
        1.0 / sqrt(1.0 + 3.0*deviationSquared / Math.PI**2)

    /// <summary>
    /// Calculates <c>E(µ, µj, φj)</c>, which is used as a component of game variance within a period.
    /// </summary>
    /// <param name="rating">The rating of the current player.</param>
    /// <param name="opponentRating">The rating of the player's opponent.</param>
    /// <param name="opponentDeviation">The opponent's rating deviation.</param>
    /// <returns><c>E(µ, µj, φj)</c></returns>
    let Three_E (rating: float, opponentRating: float, opponentDeviation: float) = 
        1.0 / (1.0 + exp(-Three_g(opponentDeviation) * (rating - opponentRating)))

    /// <summary>
    /// calculateVarianceFromGameOutcomes calculates <c>𝒱</c>, which is a player's variance within a period solely from game outcomes. 
    /// Equivalent to the entirety of step 3.
    /// </summary>
    /// <param name="playerRating">The rating of the player to calculate the variance of based on matches against all opponents.</param>
    /// <param name="opponentRatings">An array of all opponent ratings.</param>
    /// <param name="opponentDeviations">An array of all opponent rating deviations.</param>
    /// <returns>The deviation of the player when a match has been played once for each opponent rating and deviation.</returns>
    /// <exception cref="ArgumentException">If the number of opponent ratings and deviations do not match.</exception>
    let varianceFromGameOutcomes (playerRating: float, opponentRatings: float[], opponentDeviations: float[]) = 
        if opponentRatings.Length <> opponentDeviations.Length then
            failwithf "`opponentRatings` and `opponentDeviations` must be of the same length (got lengths %d and %d)" opponentRatings.Length opponentDeviations.Length

        let mutable varianceSum = 0.0

        for i in 1..opponentRatings.Length do
            let matchE = Three_E(playerRating, opponentRatings[i], opponentDeviations[i])
            varianceSum <- varianceSum + Three_g(opponentDeviations[i]) ** 2 * matchE * (1.0 - matchE)
        varianceSum
    
    /// <summary>
    /// Calculates <c>∆</c>, which represents the estimated change in rating compared to the pre-period rating. Equivalent to step 4.
    /// </summary>
    /// <param name="playerRating">The current rating of the player to estimate change of during this period.</param>
    /// <param name="opponentRatings">The ratings of all opponents played.</param>
    /// <param name="opponentDeviations">The deviations of all opponents played.</param>
    /// <param name="gameOutcomes">The outcomes of all games played.</param>
    /// <param name="periodVariance"></param>
    /// <returns>The estimated rating improvement for the player after all matches have been played</returns>
    let estimateRatingImprovement (playerRating: float, opponentRatings: float[], opponentDeviations: float[], gameOutcomes: float[], periodVariance: float) = 
        if opponentRatings.Length <> opponentDeviations.Length || opponentRatings.Length <> gameOutcomes.Length || opponentDeviations.Length <> gameOutcomes.Length then
            failwithf "`opponentRatings`, `opponentDeviations` and `gameOutcomes` must be of the same length (got lengths %d, %d and %d)" opponentRatings.Length opponentDeviations.Length gameOutcomes.Length

        let mutable ratingSum = 0.0

        for i in 1..opponentRatings.Length do
            ratingSum <- ratingSum + Three_g(opponentDeviations[i]) * (gameOutcomes[i] - Three_E(playerRating, opponentRatings[i], opponentDeviations[i]))

        ratingSum




