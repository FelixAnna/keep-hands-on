using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HSS.SignalRDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        [HttpGet]
        [Route("get")]
        [Authorize]
        public IActionResult GetToken()
        {
            return Ok(HttpContext.GetTokenAsync("access_token").Result);
        }

        [HttpGet("authorized")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public string Authorize()
        {
            return "welcome";
        }
    }
}
