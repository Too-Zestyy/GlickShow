/// <summary>
/// Controls redaction and management of player data sent to clients after being collected from the DB.
/// In addition to removing fields containing potentially sensitive data,
/// the version of glicko scale is specified for the constructor to match when returned.
/// </summary>
public class RedactedGlickoPlayer
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int SystemId { get; set; }

    public GlickoVersion RatingVersion { get; set; }
    public double Rating { get; set; }
    public double Deviation { get; set; }
    public double Volatility { get; set; }

    public RedactedGlickoPlayer(
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
