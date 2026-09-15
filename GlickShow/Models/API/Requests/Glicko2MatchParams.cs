using System.ComponentModel.DataAnnotations;

public class Glicko2MatchParams
{
    public required Glicko2Player PlayerOne { get; set; }
    public required Glicko2Player PlayerTwo { get; set; }

    [Range(0.0, 1.0)]
    public required double GameOutcome { get; set; }
}
