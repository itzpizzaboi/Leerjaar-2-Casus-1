using System.Data.Entity;
using webapi.Models;

namespace webapi.Db;

public class EnergyContext : DbContext
{
    public EnergyContext() : base("EnergyContext") { }

    public DbSet<User> Users { get; set; }
    public DbSet<EnergyDevice> EnergyDevices { get; set; }
}