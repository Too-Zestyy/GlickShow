namespace GlickoCalc

module Convert = 
    /// <summary>Converts a glicko two rating onto the glicko one rating scale.</summary>
    /// <param name="glickoTwoRating">The glicko two rating to convert.</param>
    /// <returns><see cref="glickoTwoRating"/> mapped onto the glicko one rating scale.</returns>
    let ToGlickoOneRating (glickoTwoRating: float) = 
        glickoTwoRating*173.7178 + 1500.0
    
    /// <summary>Converts a glicko two rating deviation onto the glicko one deviation scale.</summary>
    /// <param name="glickoTwoDeviation">The glicko two rating deviation to convert,</param>
    /// <returns><see cref="glickoTwoDeviation"/> mapped onto the glicko one rating deviation scale.</returns>
    let ToGlickoOneDeviation (glickoTwoDeviation: float) = 
        glickoTwoDeviation * 173.7178

    /// <summary>Converts a glicko one rating onto the glicko two rating scale.</summary>
    /// <param name="glickoOneRating">The glicko one rating to convert.</param>
    /// <returns><see cref="glickoOneRating"/> mapped onto the glicko two rating scale.</returns>
    let ToGlickoTwoRating (glickoOneRating: float) = 
        (glickoOneRating - 1500.0) / 173.7178
    
    /// <summary>Converts a glicko one rating deviation onto the glicko two deviation scale.</summary>
    /// <param name="glickoOneDeviation">The glicko one rating deviation to convert.</param>
    /// <returns><see cref="glickoOneDeviation"/> mapped onto the glicko two rating deviation scale.</returns>
    let ToGlickoTwoDeviation (glickoOneDeviation: float) = 
        glickoOneDeviation / 173.7178

    