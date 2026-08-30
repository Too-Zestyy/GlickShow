module GlickoCalc.Tests

open NUnit.Framework

[<SetUp>]
let Setup () =
    ()

[<Test>]
let TeestWinningIncreasesRating () =
    let p1Rating, p2Rating = Constants.DefaultPlayerRating, Constants.DefaultPlayerRating
    let p1Deviation, p2Deviation = Constants.DefaultPlayerDeviation, Constants.DefaultPlayerDeviation
    let p1Volatility, p2Volatility = Constants.DefaultPlayerVolatility, Constants.DefaultPlayerVolatility


    let newRating, newDeviation, newVolatility = Steps.UpdatePlayerFromMatches(
        p1Rating, p1Deviation, p1Volatility, 
        [|p2Rating|], [|p2Deviation|], [|Constants.Win|], 
        Constants.DefaultSystemConstant, Constants.DefaultConvergenceTolerance)
    printf "%f\n" (Convert.ToGlickoOneRating p1Rating)
    printf "%f" (Convert.ToGlickoOneRating newRating)

    // TODO: all new figures output are zero - look into algorithm to fix

    Assert.That(newRating, Is.GreaterThan(p1Rating))
