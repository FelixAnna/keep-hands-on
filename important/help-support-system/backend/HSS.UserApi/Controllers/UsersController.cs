using HSS.SharedServices.Contacts.Contracts;
using HSS.SharedServices.Contacts.Services;
using HSS.UserApi.Users.Contracts;
using HSS.UserApi.Users.Services;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HSS.UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IContactService contactService;

        public UsersController(IUserService userService, IContactService contactService)
        {
            this.userService = userService;
            this.contactService = contactService;
        }

        [HttpPost("login")]
        public async Task<LoginResponse> Login(LoginRequest request)
        {
            return await userService.PasswordSignInAsync(request);
        }

        [Authorize]
        [HttpGet("contact")]
        public GetContactResponse GetContacts()
        {
           var userId = User.Claims.FirstOrDefault(x=>x.Type == ClaimTypes.NameIdentifier)?.Value!;
            return contactService.GetUserContact(userId);
        }
    }
}
