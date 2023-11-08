namespace webapi.Models;
public class EnergyBuilding
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public EnergyCrib[] Cribs { get; set; }

    public EnergyBuilding() { }
}
