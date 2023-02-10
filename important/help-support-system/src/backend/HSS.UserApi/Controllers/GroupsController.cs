using HSS.SharedServices.Groups.Contracts;
using HSS.SharedServices.Groups.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HSS.UserApi.Controllers
{
    [Authorize(Policy = "Customer")]
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService groupService;

        public GroupsController(IGroupService groupService)
        {
            this.groupService = groupService;
        }

        [HttpGet("{groupId}")]
        public GetGroupMemebersResponse GetGroupMembers(string groupId)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;
            return groupService.GetGroupMembers(userId, groupId);
        }
    }
}
