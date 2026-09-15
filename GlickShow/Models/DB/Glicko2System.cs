using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NodaTime;

public class Glicko2System
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public double Constant { get; set; }
    public required Duration PeriodDuration { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Instant Epoch { get; set; }
}
