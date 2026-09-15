using NodaTime;

public class SimpleDuration
{
    public int? Days { get; set; }
    public int? Hours { get; set; }
    public int? Minutes { get; set; }
    public int? Seconds { get; set; }

    public Duration ToDuration()
    {
        return Duration.FromSeconds(Seconds ?? 0)
            + Duration.FromMinutes(Minutes ?? 0)
            + Duration.FromHours(Hours ?? 0)
            + Duration.FromDays(Days ?? 0);
    }
}
