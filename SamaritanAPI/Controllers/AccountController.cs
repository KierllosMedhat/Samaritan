using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using SamaritanAPI.Authentication;

namespace SamaritanAPI.Controllers
{
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
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                var result = await userManger.CreateAsync(user, model.Password);
                // If the user is successfully created, return ok
                if(result.Succeeded)
                {
                    var token = GenerateToken(model.UserName);
                    return Ok(new { token });
                }
                // if there are any errors, add them to the modelstate object 
                //and return the error to the client 
                foreach(var error in result.Errors)
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
                        var token = GenerateToken(model.UserName);
                        return Ok(new { token });
                    }
                }
                //If user is not found, display error message
                ModelState.AddModelError("","Invalid UserName Or Password!");
            }
            return BadRequest(ModelState);
        }

        private string? GenerateToken(string userName)
        {
            var secret = configuration["Jwt:Secret"];
            var issuer = configuration["Jwt:ValidIssuer"];
            var audience = configuration["Jwt:ValidAudiences"];
            
            if(secret is null || issuer is null || audience is null)
                throw new ApplicationException("Jwt is not set in the configuration");
            
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new []
                {
                    new Claim(ClaimTypes.Name, userName)
                }),
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