using System.Text;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace webapi.Models;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }

    [Required, Index(IsUnique = true), StringLength(320)] public string Email { get; set; }
    [Required, JsonIgnore] public byte[] PasswordHash { get; set; }
    [Required] public Dictionary<string, string> Settings { get; set; }
    [Required] public virtual ICollection<EnergyBuilding> Buildings { get; set; }
    [Required] public virtual ICollection<EnergyDevice> SubmittedDevices { get; set; }

    public User(string email, string password)
    {
        Email = email;
        PasswordHash = _hashString(password);
        Settings = new Dictionary<string, string>();
        Buildings = new List<EnergyBuilding>();
        SubmittedDevices = new List<EnergyDevice>();
    }

    public User() { }

    public bool TestPassword(string testPassword)
    {
        byte[] testHash;
        bool isPasswordCorrect;

        testHash = _hashString(testPassword);
        isPasswordCorrect = PasswordHash.SequenceEqual(testHash);

        return isPasswordCorrect;
    }

    public void SetPassword(string password)
    {
        PasswordHash = _hashString(password);
    }

    private byte[] _hashString(string password)
    {
        return SHA256.HashData(Encoding.UTF8.GetBytes(password));
    }
}