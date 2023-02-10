using HSS.SharedServices.Contacts.Contracts;
using HSS.SharedServices.Contacts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HSS.UserApi.Controllers
{
    [Authorize(Policy = "Customer")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IContactService contactService;

        public UsersController(IContactService contactService)
        {
            this.contactService = contactService;
        }

        [HttpGet("contact")]
        public GetContactResponse GetContacts()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;
            return contactService.GetUserContact(userId);
        }

        [HttpGet("colleagues")]
        public GetColleagueResponse GetUserColleagues(string? keywords = "")
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;
            return contactService.GetColleagues(userId, keywords);
        }
    }
}
