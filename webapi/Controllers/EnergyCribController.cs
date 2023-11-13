using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using System.Text;
using webapi.Helpers;
using webapi.Models;
using webapi;
using Microsoft.IdentityModel.Tokens;
using System.Collections;

namespace webapi.Controllers;

[ApiController]
[Route("user/buildings/{energyBuildingGuid}/cribs")]
[Authorize]
public class EnergyCribController : ControllerBase
{
    private readonly ILogger<EnergyCribController> _logger;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IConfiguration _config;
    private EnergyContext db = new EnergyContext();
    private readonly User user;
    private readonly EnergyBuilding energyBuilding;

    public EnergyCribController(ILogger<EnergyCribController> logger, IConfiguration config, IHttpContextAccessor httpContext, Guid energyBuildingGuid)
    {
        _logger = logger;
        _httpContext = httpContext;
        _config = config;
        user = Cookie.GetCurrentUser(_config, _httpContext, db);
        energyBuilding = user.Buildings.Where(b => (b.Id == energyBuildingGuid)).First();
    }

    [HttpGet]
    public IEnumerable<EnergyCrib> GetEnergyCribs()
    {
        return energyBuilding.Cribs;
    }

    [HttpPost]
    public Guid PostEnergyCrib(string name)
    {
        EnergyCrib crib;

        // db does not allow for null values
        if (name.IsNullOrEmpty())
            return Guid.Empty;

        crib = new EnergyCrib(name);

        energyBuilding.Cribs.Add(crib);
        db.SaveChanges();

        return crib.Id;
    }

    [Route("{energyCribGuid}")]
    [HttpGet]
    public EnergyCrib GetEnergyCrib(Guid energyCribGuid)
    {
        EnergyCrib crib;

        crib = energyBuilding.Cribs.Where(b => (b.Id == energyCribGuid)).First();

        return crib;
    }

    [Route("{energyCribGuid}")]
    [HttpPatch]
    public EnergyCrib PatchEnergyCrib(Guid energyCribGuid, string name)
    {
        EnergyCrib crib;

        crib = energyBuilding.Cribs.Where(b => (b.Id == energyCribGuid)).First();
        if (!name.IsNullOrEmpty()) crib.Name = name;

        db.SaveChanges();

        return crib;
    }

    [Route("{energyCribGuid}")]
    [HttpDelete]
    public EnergyCrib DeleteEnergyBuilding(Guid energyCribGuid)
    {
        EnergyCrib crib;

        crib = energyBuilding.Cribs.Where(b => (b.Id == energyCribGuid)).First();
        
        energyBuilding.Cribs.Remove(crib);
        db.SaveChanges();

        return crib;
    }
}

[ApiController]
[Route("user/buildings/{energyBuildingGuid}/cribs/{energyCribGuid}/devices")]
[Authorize]
public class EnergyCribDeviceController : ControllerBase
{
    private readonly ILogger<EnergyCribDeviceController> _logger;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IConfiguration _config;
    private EnergyContext db = new EnergyContext();
    private readonly User user;
    private readonly EnergyBuilding energyBuilding;
    private readonly EnergyCrib energyCrib;

    public EnergyCribDeviceController(ILogger<EnergyCribDeviceController> logger, IConfiguration config, IHttpContextAccessor httpContext, Guid energyBuildingGuid, Guid energyCribGuid)
    {
        _logger = logger;
        _httpContext = httpContext;
        _config = config;
        user = Cookie.GetCurrentUser(_config, _httpContext, db);
        energyBuilding = user.Buildings.Where(b => (b.Id == energyBuildingGuid)).First();
        energyCrib = energyBuilding.Cribs.Where(c => (c.Id == energyCribGuid)).First();
    }

    [HttpGet]
    public IEnumerable<EnergyDevice> GetEnergyCribDevices()
    {
        return energyCrib.Devices;
    }

    [HttpPost]
    public Guid PostEnergyCribDevice(Guid energyDeviceGuid)
    {
        EnergyDevice device;

        device = db.EnergyDevices.Find(energyDeviceGuid);
        energyCrib.Devices.Add(device);
        db.SaveChanges();

        return device.Id;
    }

    [Route("{energyDeviceGuid}")]
    [HttpDelete]
    public EnergyDevice DeleteEnergyCribDevice(Guid energyDeviceGuid)
    {
        EnergyDevice device;

        device = energyCrib.Devices.Where(d => (d.Id == energyDeviceGuid)).First();

        energyCrib.Devices.Remove(device);
        db.SaveChanges();

        return device;
    }
}
