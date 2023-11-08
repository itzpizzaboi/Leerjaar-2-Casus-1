namespace webapi.Models;
public class EnergySnapshot
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public UInt32 WattagePerHour { get; set; }

    public EnergySnapshot() { }
}
