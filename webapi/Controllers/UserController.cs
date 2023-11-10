using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Helpers;
using webapi.Models;
using webapi.Db;

namespace webapi.Controllers;

[ApiController]
[Route("user")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IConfiguration _config;
    private EnergyContext db = new EnergyContext();
    private readonly User user;

    public UserController(ILogger<UserController> logger, IConfiguration config, IHttpContextAccessor httpContext)
    {
        _logger = logger;
        _httpContext = httpContext;
        _config = config;
        user = Cookie.GetCurrentUser(_config, _httpContext, db);
    }

    [HttpGet]
    public User GetUser()
    {
        return user;
    }

    [HttpDelete]
    public bool DeleteUser()
    {
        db.Users.Remove(user);
        db.SaveChanges();

        return true;
    }

    [Route("devices")]
    [HttpGet]
    public IEnumerable<EnergyDevice> GetUserSubmittedDevices()
    {
        ICollection<EnergyDevice> devices;

        devices = user.SubmittedDevices;

        return devices;
    }
}