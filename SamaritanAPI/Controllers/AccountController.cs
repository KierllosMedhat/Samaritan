using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SamaritanAPI.Authentication;

namespace SamaritanAPI.Controllers
{
    [Authorize(Roles =$"{AppRoles.Administrator}")]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> userManger;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<AppUser> userManger
            ,IConfiguration configuration)
        {
            this.configuration = configuration;
            this.userManger = userManger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AddOrUpdateAppUserModel model)
        {
            if(ModelState.IsValid)
            {
                var existingUser = await userManger.FindByNameAsync(model.UserName);
                if(existingUser != null)
                {
                    ModelState.AddModelError("", "UserName is already taken");
                    return BadRequest(ModelState);
                }
                // Create a new user
                var user = new AppUser()
                {
                    UserName = model.UserName,
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    Status = Status.Offline,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                var userResult = await userManger.CreateAsync(user, model.Password);
                var roleResult = await userManger.AddToRoleAsync(user, model.Role);
                // If the user is successfully created, return ok
                if(userResult.Succeeded && roleResult.Succeeded)
                {
                    var token = GenerateToken(model.UserName, user);
                    return Ok(new { token });
                }
                // if there are any errors, add them to the modelstate object 
                //and return the error to the client 
                foreach(var error in userResult.Errors)
                    ModelState.AddModelError("", error.Description);
                foreach(var error in roleResult.Errors)
                    ModelState.AddModelError("", error.Description);
                // if we got this far, something failed, redisplay form
            }
            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await userManger.FindByNameAsync(model.UserName);
                if(user != null)
                {
                    if(await userManger.CheckPasswordAsync(user, model.Password))
                    {
                        var token = GenerateToken(model.UserName, user);
                        return Ok(new { token });
                    }
                }
                //If user is not found, display error message
                ModelState.AddModelError("","Invalid UserName Or Password!");
            }
            return BadRequest(ModelState);
        }

        private async Task<string?> GenerateToken(string userName, AppUser user)
        {
            var secret = configuration["Jwt:Secret"];
            var issuer = configuration["Jwt:ValidIssuer"];
            var audience = configuration["Jwt:ValidAudiences"];
            
            if(secret is null || issuer is null || audience is null)
                throw new ApplicationException("Jwt is not set in the configuration");
            
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var tokenHandler = new JwtSecurityTokenHandler();
            var userRoles = await userManger.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, userName)
            };
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            return token;
        }
    }
}