using EStore.Common.Exceptions;
using EStore.UserAPI.Users.Contracts;
using EStore.UserAPI.Users.Services;
using Microsoft.AspNetCore.Mvc;

namespace EStore.UserAPI.Controllers
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
