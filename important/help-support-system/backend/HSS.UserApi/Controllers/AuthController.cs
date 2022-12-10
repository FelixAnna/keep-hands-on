using HSS.UserApi.Users.Contracts;
using HSS.UserApi.Users.Services;
using Microsoft.AspNetCore.Mvc;

namespace HSS.UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService userService;

        public AuthController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("login")]
        public async Task<LoginResponse> Login(LoginRequest request)
        {
            return await userService.PasswordSignInAsync(request);
        }
    }
}
