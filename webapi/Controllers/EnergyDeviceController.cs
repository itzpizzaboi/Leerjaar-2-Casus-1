using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Helpers;
using webapi.Models;
using webapi;
using Microsoft.IdentityModel.Tokens;

namespace webapi.Controllers;

[ApiController]
[Route("devices")]
public class EnergyDeviceController : ControllerBase
{
    private readonly ILogger<EnergyDeviceController> _logger;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IConfiguration _config;
    private EnergyContext db = new EnergyContext();

    public EnergyDeviceController(ILogger<EnergyDeviceController> logger, IConfiguration config, IHttpContextAccessor httpContext)
    {
        _logger = logger;
        _httpContext = httpContext;
        _config = config;
    }

    [HttpGet]
    public IEnumerable<EnergyDevice> GetEnergyDevices()
    {
        return db.EnergyDevices;
    }

    [HttpPost]
    [Authorize]
    public Guid PostEnergyDevice(string name, uint wattagePerHour, byte[]? banner = null)
    {
        EnergyDevice device;
        User user;

        user = _getCurrentUser();

        _logger.LogError($"wattagePerHour: {wattagePerHour}");

        // db does not allow for null values
        if (banner.IsNullOrEmpty())
            banner = Array.Empty<byte>();

        device = new EnergyDevice(name, wattagePerHour, banner);

        _logger.LogError($"device.WattagePerHour: {device.WattagePerHour}");

        user.SubmittedDevices.Add(device);
        db.EnergyDevices.Add(device);
        db.SaveChanges();

        return device.Id;
    }

    [Route("{eventGuid}")]
    [HttpGet]
    public EnergyDevice GetEnergyDevice(Guid energyDeviceGuid)
    {
        return db.EnergyDevices.Find(energyDeviceGuid);
    }

    private User _getCurrentUser()
    {
        return Cookie.GetCurrentUser(_config, _httpContext, db);
    }
}
