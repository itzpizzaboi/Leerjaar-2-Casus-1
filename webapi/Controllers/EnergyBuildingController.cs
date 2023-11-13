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
[Route("user/buildings")]
[Authorize]
public class EnergyBuildingController : ControllerBase
{
    private readonly ILogger<EnergyBuildingController> _logger;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IConfiguration _config;
    private EnergyContext db = new EnergyContext();
    private readonly User user;

    public EnergyBuildingController(ILogger<EnergyBuildingController> logger, IConfiguration config, IHttpContextAccessor httpContext)
    {
        _logger = logger;
        _httpContext = httpContext;
        _config = config;
        user = Cookie.GetCurrentUser(_config, _httpContext, db);
    }

    [HttpGet]
    public IEnumerable<EnergyBuilding> GetEnergyBuildings()
    {
        return user.Buildings;
    }

    [HttpPost]
    public Guid PostEnergyBuilding(string name)
    {
        EnergyBuilding building;

        // db does not allow for null values
        if (name.IsNullOrEmpty())
            return Guid.Empty;

        building = new EnergyBuilding(name);

        user.Buildings.Add(building);
        db.SaveChanges();

        return building.Id;
    }

    [Route("{energyBuildingGuid}")]
    [HttpGet]
    public EnergyBuilding GetEnergyBuilding(Guid energyBuildingGuid)
    {
        EnergyBuilding building;

        building = user.Buildings.Where(b => (b.Id == energyBuildingGuid)).First();

        return building;
    }

    [Route("{energyBuildingGuid}")]
    [HttpPatch]
    public EnergyBuilding PatchEnergyBuilding(Guid energyBuildingGuid, string name)
    {
        EnergyBuilding building;

        building = user.Buildings.Where(b => (b.Id == energyBuildingGuid)).First();
        if (!name.IsNullOrEmpty()) building.Name = name;

        db.SaveChanges();

        return building;
    }

    [Route("{energyBuildingGuid}")]
    [HttpDelete]
    public EnergyBuilding DeleteEnergyBuilding(Guid energyBuildingGuid)
    {
        EnergyBuilding building;

        building = user.Buildings.Where(b => (b.Id == energyBuildingGuid)).First();
        
        user.Buildings.Remove(building);
        db.SaveChanges();

        return building;
    }
}
