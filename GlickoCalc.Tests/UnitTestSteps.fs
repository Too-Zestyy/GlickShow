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

    Assert.Multiple( fun () ->
        Assert.That(System.Math.Round(Convert.ToGlickoOneRating p2NRating, 2), Is.EqualTo 1337.69, "The losing player did not have their rating updated as expected.")
        Assert.That(round (Convert.ToGlickoOneDeviation p2NDeviation), Is.EqualTo 290, "The losing player did not have their deviation updated as expected.")
        Assert.That(p2Volatility, Is.EqualTo 0.05999968, "The losing player did not have their volatility updated as expected.")
    )
    

// TODO: Add example from glicko 2 paper
[<Test>]
let TestImplementationMatchesPaperExample () = 
    // While some of the figures used in this test case already exist within `Constants`, all numbers have been hardcoded to maintain consistency with the paper if 
    // the constants are ever changed.

    let p1Rating, p1Deviation, p1Volatility = Convert.ToGlickoTwoRating 1500.0, Convert.ToGlickoTwoDeviation 200.0, 0.06

    let p1NRating, p1NDeviation, p1NVolatility = Steps.UpdatePlayerFromMatches(
        p1Rating, p1Deviation, p1Volatility, 
        [|Convert.ToGlickoTwoRating 1400.0; Convert.ToGlickoTwoRating 1550.0; Convert.ToGlickoTwoRating 1700.0|], 
        [|Convert.ToGlickoTwoDeviation 30; Convert.ToGlickoTwoDeviation 100; Convert.ToGlickoTwoDeviation 300|], 
        [|Constants.Win; Constants.Loss; Constants.Loss|], 
        0.5, 0.000001)

    Assert.Multiple( fun () ->
        Assert.That(System.Math.Round(p1NRating, 4), Is.EqualTo -0.2069, "The player's rating does not match the paper's calculations")
        Assert.That(System.Math.Round(p1NDeviation, 4), Is.EqualTo 0.8722, "The player's deviation does not match the paper's calculations")
        Assert.That(System.Math.Round(p1NVolatility, 5), Is.EqualTo 0.05999, "The player's volatility does not match the paper's calculations")
    )
