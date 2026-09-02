using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.SignalR;

public class Glicko2MatchParameters
{
    public required Glicko2Player PlayerOne { get; set; }
    public required Glicko2Player PlayerTwo { get; set; }
    
    [Range(0.0, 1.0)]
    public required double GameOutcome { get; set; }
}