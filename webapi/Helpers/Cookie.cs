using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using webapi.Models;
using webapi.Db;

namespace webapi.Helpers;

static public class Cookie
{

    static public void SetCookie(IConfiguration config, IHttpContextAccessor httpContext, string cookieName, string value)
    {
        CookieOptions cookieOptions;

        cookieOptions = new CookieOptions();
        cookieOptions.Expires = DateTime.Now.AddSeconds(Double.Parse(config["Jwt:ExpirationTime"]));
        cookieOptions.Path = "/";
        cookieOptions.HttpOnly = true;

        httpContext.HttpContext.Response.Cookies.Append(cookieName, value, cookieOptions);
    }

    static public string GetCookie(IHttpContextAccessor httpContext, string cookieName)
    {
        return httpContext.HttpContext.Request.Cookies[cookieName];
    }

    static public void SetCurrentUser(IConfiguration config, IHttpContextAccessor httpContext, User user)
    {
        string jwtString;
        string userId;

        userId = user.Id.ToString();

        jwtString = _encodeUserJwt(config, userId);
        SetCookie(config, httpContext, config["Jwt:CookieName"], jwtString);
    }

    static public User GetCurrentUser(IConfiguration config, IHttpContextAccessor httpContext, EnergyContext db)
    {
        string jwtString;
        Dictionary<string, string> jwtDict;
        Guid userGuid;
        User user;

        jwtString = GetCookie(httpContext, config["Jwt:CookieName"]);
        jwtDict = _decodeUserJwt(config, jwtString);

        userGuid = Guid.Parse(jwtDict["id"]);
        //user = db.Users.Include("Events").First(u => (u.Id == userGuid));
        user = db.Users.Find(userGuid);

        return user;
    }

    static public bool UnsetCurrentUser(IConfiguration config, IHttpContextAccessor httpContext)
    {
        httpContext.HttpContext.Response.Cookies.Delete(config["Jwt:CookieName"]);

        return true;
    }

    static private string _encodeUserJwt(IConfiguration config, string id)
    {
        var key = Encoding.ASCII.GetBytes(config["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {
                new Claim("id", id),
            }),
            Expires = DateTime.UtcNow.AddSeconds(Double.Parse(config["Jwt:ExpirationTime"])),
            Issuer = config["Jwt:Issuer"],
            Audience = config["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtString = tokenHandler.WriteToken(token);

        return jwtString;
    }

    static private Dictionary<string, string> _decodeUserJwt(IConfiguration config, string jwtString)
    {
        var key = Encoding.ASCII.GetBytes(config["Jwt:Key"]);
        var validationParameters = new TokenValidationParameters()
        {
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidAudience = config["Jwt:Issuer"],
            ValidIssuer = config["Jwt:Audience"],
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken validatedToken = null;

        // errors when validation error
        ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(jwtString, validationParameters, out validatedToken);

        var claimsDictionary = new Dictionary<string, string>
        {
            { "id", claimsPrincipal.FindFirst("id").Value },
        };

        return claimsDictionary;
    }
}