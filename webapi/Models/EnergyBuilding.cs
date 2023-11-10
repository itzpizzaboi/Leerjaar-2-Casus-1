using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace webapi.Models;
public class EnergyBuilding
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }
    [Required] public string Name { get; set; }
    [Required] public virtual ICollection<EnergyCrib> Cribs { get; set; }

    public EnergyBuilding(string name)
    {
        Name = name;
    }

    public EnergyBuilding() { }
}
