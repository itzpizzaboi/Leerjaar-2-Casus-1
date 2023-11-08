using Microsoft.AspNetCore.Mvc;
using webapi.Helpers;
using webapi.Models;
using webapi.Db;
using System.Data.Entity;

namespace webapi.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IConfiguration _config;
    private readonly IHttpContextAccessor _httpContext;
    private EnergyContext db = new EnergyContext();

    public AuthController(ILogger<AuthController> logger, IConfiguration config, IHttpContextAccessor httpContext)
    {
        _logger = logger;
        _config = config;
        _httpContext = httpContext;
    }

    [Route("login")]
    [HttpPut]
    public bool PutLogin(string email, string password)
    {
        User? user;

        user = db.Users.Where(u => u.Email == email).FirstOrDefault();
        if (user == null)
            return false;

        if (user.TestPassword(password) == false)
            return false;

        Cookie.SetCurrentUser(_config, _httpContext, user);

        return true;
    }

    [Route("signup")]
    [HttpPost]
    public bool PostSignup(string email, string password)
    {
        User user;

        user = new User(email, password);

        db.Users.Add(user);
        
        try
        {
            db.SaveChanges();
        } catch {
            // constraint failed
            db.Entry(user).State = EntityState.Detached;
            return false;
        }

        Cookie.SetCurrentUser(_config, _httpContext, user);

        return true;
    }

    [Route("logout")]
    [HttpDelete]
    public bool DeleteLogout()
    {
        Cookie.UnsetCurrentUser(_config, _httpContext);

        return true;
    }
}