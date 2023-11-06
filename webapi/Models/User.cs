namespace webapi.Models;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    byte[] Password { get; set; }
    public Dictionary<string, string> Parameters { get; set; }
    public EnergyBuilding[] Buildings { get; set; }
    public EnergyDevice[] SubmittedDevices { get; set; }
}