/// <summary>
/// Removes the references to the internal user and system elements
/// for response payloads from a player entity for both brevity and security purposes.
/// </summary>
public class RedactedGlicko2Player
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int SystemId { get; set; }

    public GlickoVersion RatingVersion { get; set; }
    public double Rating { get; set; }
    public double Deviation { get; set; }
    public double Volatility { get; set; }

    public RedactedGlicko2Player(
        Glicko2Player player,
        GlickoVersion glickoVersion
    )
    {
        Id = player.Id;
        UserId = player.AppUserId;
        SystemId = player.SystemId;

        RatingVersion = glickoVersion;
        if (glickoVersion == GlickoVersion.Two)
        {
            Rating = player.Rating;
            Deviation = player.Deviation;
        }
        else
        {
            Rating = GlickoCalc.Convert.ToGlickoOneRating(player.Rating);

            Deviation = GlickoCalc.Convert.ToGlickoOneDeviation(
                player.Deviation
            );
        }

        Volatility = player.Volatility;
    }
}
