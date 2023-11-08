using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace webapi.Models;

public class EnergyDevice
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }

    [Required] public string Name { get; set; }
    [Required] public uint WattagePerHour { get; set; }
    [Required] public byte[] Banner { get; set; }

    public EnergyDevice(string name, uint wattagePerHour, byte[] banner)
    {
        Name = name;
        WattagePerHour = wattagePerHour;
        Banner = banner;
    }

    public EnergyDevice() { }
}