using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace webapi.Models;
public class EnergySnapshot
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }
    [Required] public DateTime Timestamp { get; set; }
    [Required] public uint WattagePerHour { get; set; }

    public EnergySnapshot(DateTime timestamp, uint wattagePerHour)
    {
        Timestamp = timestamp;
        WattagePerHour = wattagePerHour;
    }

    public EnergySnapshot() { }
}
