using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

namespace SingleRDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        //private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration config;

        public TokenController(/*SignInManager<IdentityUser> signInManager,*/ IConfiguration config)
        {
            //this.signInManager = signInManager;
            this.config = config;
        }

        [HttpGet]
        [Route("get")]
        [Authorize]
        public IActionResult GetToken()
        {
            return Ok(HttpContext.GetTokenAsync("access_token").Result);
            //return Ok(GenerateToken(User.Identity.Name));
        }

        private string GenerateToken(string userId)
        {
            var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(config["JwtKey"]));

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            };

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken("signalrdemo", "signalrdemo", claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }        
        // use for non-cookie access
        //[HttpPost("api/token")]
        //public async Task<IActionResult> GetTokenForCredentialsAsync([FromBody] LoginRequest login)
        //{
        //    var result = await signInManager.PasswordSignInAsync(login.Username, login.Password, false, true);
        //    return result.Succeeded ? (IActionResult)Ok(GenerateToken(login.Username)) : Unauthorized();
        //}
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
