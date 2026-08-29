namespace GlickoCalc
open System

module Steps =
    let Three_g (deviation: float) = 
        let deviationSquared = deviation ** 2 
        1.0 / sqrt(1.0 + 3.0*deviationSquared / Math.PI**2)