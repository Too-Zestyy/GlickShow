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
    /// <returns>The variance of the player when a match has been played once for each opponent rating and deviation.</returns>
    /// <exception cref="ArgumentException">If the number of opponent ratings and deviations do not match.</exception>
    let varianceFromGameOutcomes (playerRating: float, opponentRatings: float[], opponentDeviations: float[]) = 
        if opponentRatings.Length <> opponentDeviations.Length then
            failwithf "`opponentRatings` and `opponentDeviations` must be of the same length (got lengths %d and %d)" opponentRatings.Length opponentDeviations.Length

        let mutable varianceSum = 0.0

        for i in 0..opponentRatings.Length-1 do
            let matchE = Three_E(playerRating, opponentRatings[i], opponentDeviations[i])
            varianceSum <- varianceSum + Three_g(opponentDeviations[i]) ** 2 * matchE * (1.0 - matchE)
        
        1.0 / varianceSum
    
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

        for i in 0..opponentRatings.Length-1 do
            ratingSum <- ratingSum + Three_g(opponentDeviations[i]) * (gameOutcomes[i] - Three_E(playerRating, opponentRatings[i], opponentDeviations[i]))

        periodVariance * ratingSum

    /// <summary>Calculates <c>a</c>, which is used as a component for updating volatility in multiple places.</summary>
    /// <param name="volatility">The original volatility which will be updated</param>
    /// <returns><c>a</c></returns>
    let aFromVolatility (volatility: float) = 
        log(volatility ** 2)

    /// <summary>Calculates <c>f(x)</c> within step 5, which is used during the iterative procedure when updating volatility.</summary>
    /// <param name="x">The main input for the formula.</param>
    /// <param name="systemConstant">The current system constant.</param>
    /// <param name="volatility">The player's original performance volatility</param>
    /// <param name="delta">The player's estimated rating improvement (calculated in step 4).</param>
    /// <param name="deviation">The player's current rating deviation.</param>
    /// <param name="variance">The player's estimated variance (calculated in step 3).</param>
    /// <returns><c>f(x)</c></returns>
    let fVolatilityFunction (x: float, systemConstant: float, volatility: float, delta: float, deviation: float, variance: float) = 
        let a = aFromVolatility volatility
        let ePowX = exp x
        let deviationSquared = deviation ** 2

        ePowX * (delta ** 2 - deviationSquared - variance - ePowX) / (2.0 * (deviationSquared + variance + ePowX) ** 2) - (x-a) / systemConstant ** 2

    /// <summary>Carries out the entire iterative procedure of step 5 to calculate the updated volatility for the period.</summary>
    /// <param name="convergenceTolerance">The system's margin of error for the newly converged volatility.</param>
    /// <param name="systemConstant">The system constant to be used across the period.</param>
    /// <param name="volatility">The player's current performance volatility.</param>
    /// <param name="delta">The player's estimated rating improvement (calculated in step 4).</param>
    /// <param name="deviation">The player's current rating deviation.</param>
    /// <param name="variance">The player's estimated variance (calculated in step 3).</param>
    /// <returns>The updated volatility figure after the matches within the period have been played.</returns>
    let calculateNewVolatility (convergenceTolerance: float, systemConstant: float, volatility: float, delta: float, deviation: float, variance: float) = 
        let a = aFromVolatility volatility

        let mutable A = a
        let mutable B = 0.0

        if delta ** 2 > deviation**2 + variance then
            B <- log(delta ** 2 - deviation ** 2 - variance)
        else 
            let mutable k = 1.0

            while fVolatilityFunction(a-k*systemConstant, systemConstant, volatility, delta, deviation, variance) < 0 do
                k <- k + 1.0

            B <- a - k*systemConstant

        let mutable fA = fVolatilityFunction(A, systemConstant, volatility, delta, deviation, variance)
        let mutable fB = fVolatilityFunction(B, systemConstant, volatility, delta, deviation, variance)

        while abs(B-A) > convergenceTolerance do

            let C = A + (A - B) * fA/ (fB - fA)
            let fC = fVolatilityFunction(C, systemConstant, volatility, delta, deviation, variance)

            if fC * fB <= 0 then
                A <- B
                fA <- fB
            else
                fA <- fA / 2.0
            
            B <- C
            fB <- fC

        exp(A / 2.0)
            
    /// <summary>Combines steps 4 and 5 to directly return updated volatility given the matches within the period.</summary>
    /// <param name="playerRating">The current rating of the player to estimate change of during this period.</param>
    /// <param name="playerDeviation">The player's current rating deviation.</param>
    /// <param name="playerVolatility">The player's current performance volatility.</param>
    /// <param name="periodVariance">The variance of the player when a match has been played once for each opponent rating and deviation.</param>
    /// <param name="opponentRatings">The ratings of all opponents played.</param>
    /// <param name="opponentDeviations">The deviations of all opponents played.</param>
    /// <param name="gameOutcomes">The outcomes of all games played.</param>
    /// <param name="convergenceTolerance">The tolerance to accept a value of volatility within.</param>
    /// <param name="systemConstant">The system constant to use for calcultions.</param>
    /// <returns>The new volatility for the player after the matches within the period have been played.</returns>
    let volatilityFromMatches (
        playerRating: float, playerDeviation: float, playerVolatility: float, periodVariance: float, 
        opponentRatings: float[], opponentDeviations: float[], gameOutcomes: float[], 
        convergenceTolerance: float, systemConstant: float) = 
        let delta = estimateRatingImprovement(playerRating, opponentRatings, opponentDeviations, gameOutcomes, periodVariance)
        calculateNewVolatility(convergenceTolerance, systemConstant, playerVolatility, delta, playerDeviation, periodVariance)

    /// <summary>Calculates the pre-period deviation for a player to be used for calculating the post-period deviation.</summary>
    /// <remarks>This step is used for all players during a period, regardless of whether they have played a game within it or not. 
    /// In that case, the player's post-period deviation is the same as the pre-period deviation.</remarks>
    /// <param name="deviation">The current rating deviation of the player.</param>
    /// <param name="volatility">The current performance volatility of the player.</param>
    /// <returns>The pre-period rating deviation of the player (<c>φ∗</c>).</returns>
    let preRatingPeriodDeviation (deviation: float, volatility: float) = 
        sqrt(deviation ** 2 + volatility ** 2)

    /// <summary>Calculates the post-period deviation for players who have played matches within the period.</summary>
    /// <param name="playerDeviation">The current rating deviation of the player.</param>
    /// <param name="variance">The variance obtained from the match results within the period.</param>
    /// <param name="newVolatility">The updated volatility given the matches within the period.</param>
    /// <returns>The post-period rating deviation of the player (<c>φ′</c>).</returns>
    let playedPeriodDeviation (playerDeviation: float, variance: float, newVolatility: float) = 
        1.0 / sqrt(1.0/preRatingPeriodDeviation(playerDeviation, newVolatility)**2 + 1.0/variance)
    
    /// <summary>Calculates the post-period rating of the player based on the matches played within the period.</summary>
    /// <param name="playerRating">The current rating of the player to estimate change of during this period.</param>
    /// <param name="postPeriodDeviation">The new rating deviation of the player after the current period has been taken into account.</param>
    /// <param name="opponentRatings">The ratings of all opponents played.</param>
    /// <param name="opponentDeviations">The deviations of all opponents played.</param>
    /// <param name="gameOutcomes">The outcomes of all games played.</param>
    /// <returns>The post-period rating of the player after matches have been taken into account (<c>μ′</c>).</returns>
    let playedPeriodRating (playerRating: float, postPeriodDeviation: float, opponentRatings: float[], opponentDeviations: float[], gameOutcomes: float[]) = 
        if opponentRatings.Length <> opponentDeviations.Length || opponentRatings.Length <> gameOutcomes.Length || opponentDeviations.Length <> gameOutcomes.Length then
            failwithf "`opponentRatings`, `opponentDeviations` and `gameOutcomes` must be of the same length (got lengths %d, %d and %d)" opponentRatings.Length opponentDeviations.Length gameOutcomes.Length

        let mutable deviationSum = 0.0

        for i in 0..opponentRatings.Length-1 do
            deviationSum <- deviationSum + Three_g(opponentDeviations[i]) * (gameOutcomes[i] - Three_E(playerRating, opponentRatings[i], opponentDeviations[i]))

        playerRating + postPeriodDeviation ** 2 * deviationSum

    /// <summary>Carries out all necessary updates/calculations necessary for a period given 
    /// the player's stats, system configuration and matches played.</summary>
    /// <remarks>Unless there is no other way to carry out a task within a project consuming this library, 
    /// <strong>this is the only function that should be directly called upon by consumers for purposes of updating player states</strong>.</remarks>
    /// <param name="playerRating">The current rating of the player to estimate change of during this period.</param>
    /// <param name="playerDeviation">The player's current rating deviation.</param>
    /// <param name="playerVolatility">The player's current performance volatility.</param>
    /// <param name="opponentRatings">The ratings of all opponents played.</param>
    /// <param name="opponentDeviations">The deviations of all opponents played.</param>
    /// <param name="gameOutcomes">The outcomes of all games played.</param>
    /// <param name="systemConstant">The system constant to use for calcultions.</param>
    /// <param name="convergenceTolerance">The tolerance to accept a value of volatility within.</param>
    /// <returns>The player's stats after the period (i.e. after the period's matches or lack thereof have been used to update player stats).</returns>
    let UpdatePlayerFromMatches (
        playerRating: float, playerDeviation: float, playerVolatility: float, 
        opponentRatings: float[], opponentDeviations: float[], gameOutcomes: float[],
        systemConstant: float, convergenceTolerance: float) = 
        if opponentRatings.Length <> opponentDeviations.Length || opponentRatings.Length <> gameOutcomes.Length || opponentDeviations.Length <> gameOutcomes.Length then
            failwithf "`opponentRatings`, `opponentDeviations` and `gameOutcomes` must be of the same length (got lengths %d, %d and %d)" opponentRatings.Length opponentDeviations.Length gameOutcomes.Length

        if opponentRatings.Length = 0 then
            // Step 6 still applies to players who have played no games to decay rating certainty
            playerRating, preRatingPeriodDeviation(playerDeviation, playerVolatility), playerVolatility
        else
            let periodVariance = varianceFromGameOutcomes(playerRating, opponentRatings, opponentDeviations)
            let newVolatility = volatilityFromMatches(
                playerRating, playerDeviation, playerVolatility, periodVariance, 
                opponentRatings, opponentDeviations, gameOutcomes, 
                convergenceTolerance, systemConstant)
            
            let newDeviation = playedPeriodDeviation(playerDeviation, periodVariance, newVolatility)

            let newRating = playedPeriodRating(playerRating, newDeviation, opponentRatings, opponentDeviations, gameOutcomes)


            newRating, newDeviation, newVolatility









