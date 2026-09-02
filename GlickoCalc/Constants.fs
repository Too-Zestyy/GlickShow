namespace GlickoCalc

module Constants = 
    let DefaultPlayerRating, DefaultPlayerDeviation, DefaultPlayerVolatility = 0.0, Convert.ToGlickoTwoDeviation 350.0, 0.06

    let LowSystemConstant, DefaultSystemConstant, HighSystemConstant = 0.3, 0.5, 1.2
    let DefaultConvergenceTolerance = 0.000001
    let Win, Draw, Loss = 1.0, 0.5, 0.0

    