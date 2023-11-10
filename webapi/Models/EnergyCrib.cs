using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace webapi.Models;
public class EnergyCrib
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }
    [Required] public string Name { get; set; }
    [Required] public virtual ICollection<EnergySnapshot> Snapshots { get; set; }
    [Required] public virtual ICollection<EnergyDevice> Devices { get; set; }

    public EnergyCrib(string name)
    {
        Name = name;
    }

    public EnergyCrib() { }
}
