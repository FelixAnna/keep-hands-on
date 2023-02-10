using HSS.SharedServices.Friends;
using HSS.SharedServices.Friends.Contracts;
using HSS.SharedServices.Friends.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HSS.UserApi.Controllers
{
    [Authorize(Policy = "Customer")]
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        private readonly IFriendService friendService;

        public FriendsController(IFriendService friendService)
        {
            this.friendService = friendService;
        }

        [HttpGet()]
        public IList<FriendModel> GetFriends()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;
            return friendService.GetFriends(userId);
        }

        [HttpPost()]
        public async Task AddFriend(AddFriendRequest request)
        {
            if (string.IsNullOrEmpty(request.UserId))
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;
                request.UserId = userId;
            }

            await friendService.AddFriendAsync(request);
        }
    }
}
