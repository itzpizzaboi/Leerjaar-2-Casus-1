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

    public UserController(ILogger<UserController> logger, IConfiguration config, IHttpContextAccessor httpContext)
    {
        _logger = logger;
        _httpContext = httpContext;
        _config = config;
    }

    [HttpGet]
    public User GetUser()
    {
        User user;

        user = _getCurrentUser();

        return user;
    }

    // TODO: implement forgot password et cetera
    /*[HttpPatch]
    public bool PatchUser(string? email, string? password)
    {
        User user;

        user = _getCurrentUser();

        if (!string.IsNullOrWhiteSpace(email)) user.Email = email;
        if (!string.IsNullOrWhiteSpace(password)) user.SetPassword(password);

        db.SaveChanges();

        return true;
    }*/

    [HttpDelete]
    public bool DeleteUser()
    {
        User user;

        user = _getCurrentUser();

        db.Users.Remove(user);
        db.SaveChanges();

        return true;
    }

    [Route("subdev")]
    [HttpGet]
    public IEnumerable<EnergyDevice> GetUserSubmittedDevices()
    {
        User user;
        ICollection<EnergyDevice> devices;

        user = _getCurrentUser();
        devices = user.SubmittedDevices;

        return devices;
    }


    // shorthand for usability
    private User _getCurrentUser()
    {
        return Cookie.GetCurrentUser(_config, _httpContext, db);
    }
}