using EStore.Common.Exceptions;
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

        [HttpGet("a")]
        public async Task Error()
        {
            throw new KSEOperationFailedException("Text Exception");
        }

        [HttpGet("b")]
        public async Task Error2()
        {
            throw new KSENotFoundException("Text Exception");
        }
    }
}
