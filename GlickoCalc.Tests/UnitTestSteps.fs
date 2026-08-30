module GlickoCalc.Tests

open NUnit.Framework
open System

let DefaultPlayerValues = Constants.DefaultPlayerRating, Constants.DefaultPlayerDeviation, Constants.DefaultPlayerVolatility

[<SetUp>]
let Setup () =
    ()

[<Test>]
let TestWinningIncreasesRating () =
    let p1Rating, p2Rating = Constants.DefaultPlayerRating, Constants.DefaultPlayerRating
    let p1Deviation, p2Deviation = Constants.DefaultPlayerDeviation, Constants.DefaultPlayerDeviation
    let p1Volatility, p2Volatility = Constants.DefaultPlayerVolatility, Constants.DefaultPlayerVolatility


    let newRating, newDeviation, newVolatility = Steps.UpdatePlayerFromMatches(
        p1Rating, p1Deviation, p1Volatility, 
        [|p2Rating|], [|p2Deviation|], [|Constants.Win|], 
        Constants.DefaultSystemConstant, Constants.DefaultConvergenceTolerance)

    // TODO: all new figures output are zero - look into algorithm to fix

    Assert.That(newRating, Is.GreaterThan(p1Rating))

[<Test>]
let TestTwoEqualPlayersHaveRatingUpdatedAsExpected () = 
    let p1Rating, p1Deviation, p1Volatility = DefaultPlayerValues
    let p2Rating, p2Deviation, p2Volatility = DefaultPlayerValues

    let p1NRating, p1NDeviation, p1NVolatility = Steps.UpdatePlayerFromMatches(
        p1Rating, p1Deviation, p1Volatility, 
        [|p2Rating|], [|p2Deviation|], [|Constants.Win|], 
        Constants.DefaultSystemConstant, Constants.DefaultConvergenceTolerance)

    let p2NRating, p2NDeviation, p2NVolatility = Steps.UpdatePlayerFromMatches(
        p2Rating, p2Deviation, p2Volatility, 
        [|p1Rating|], [|p1Deviation|], [|Constants.Loss|], 
        Constants.DefaultSystemConstant, Constants.DefaultConvergenceTolerance)
    
    Assert.That(System.Math.Round(Convert.ToGlickoOneRating p1NRating, 2), Is.EqualTo 1662.31, "The winning player did not have their rating updated as expected.")
    Assert.That(System.Math.Round(Convert.ToGlickoOneRating p2NRating, 2), Is.EqualTo 1337.69, "The losing player did not have their rating updated as expected.")

