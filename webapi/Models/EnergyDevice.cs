namespace webapi.Models;

public class EnergyDevice
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public UInt32 WattagePerHour { get; set; }
}