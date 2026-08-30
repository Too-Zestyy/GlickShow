namespace GlickoCalc

module Convert = 
    let ToGlickoOneRating (glickoTwoRating: float) = 
        glickoTwoRating*173.7178 + 1500.0
    
    let ToGlickoOneDeviation (glickoTwoDeviation: float) = 
        glickoTwoDeviation * 173.7178

    let ToGlickoTwoRating (glickoOneRating: float) = 
        (glickoOneRating - 1500.0) / 173.7178
    
    let ToGlickoTwoDeviation (glickoOneDeviation: float) = 
        glickoOneDeviation / 173.7178

    