using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Glicko2System
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }
    public required double Constant { get; set; }

    // public Glicko2System(double constant)
    // {
    //     Constant = constant;
    // }
}