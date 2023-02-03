using EStore.UserApi.Users.Contracts;
using EStore.UserApi.Users.Services;
using Microsoft.AspNetCore.Mvc;

namespace EStore.UserApi.Controllers
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
