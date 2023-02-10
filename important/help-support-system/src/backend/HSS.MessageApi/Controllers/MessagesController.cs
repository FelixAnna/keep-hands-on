using HSS.SharedServices.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HSS.MessageApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("hello")]
        public string Index2(string from)
        {

            return "Hi";
        }

        [Authorize(Policy = "Customer")]
        [HttpGet("user")]
        public List<MessageModel> Index(string from)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;
            return _messageService.GetMessages(from, userId);
        }

        [Authorize(Policy = "Customer")]
        [HttpGet("group")]
        public List<MessageModel> Group(string groupId)
        {
            return _messageService.GetGroupMessages(groupId);
        }
    }
}
