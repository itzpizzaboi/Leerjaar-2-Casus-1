namespace webapi.Models;
public class EnergyCrib
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public EnergySnapshot[] Snapshots { get; set; }
    public EnergyDevice[] Devices { get; set; }

    public EnergyCrib() { }
}
