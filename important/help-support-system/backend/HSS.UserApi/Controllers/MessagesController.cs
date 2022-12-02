using HSS.SharedServices.Messages;
using HSS.UserApi.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HSS.UserApi.Controllers
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

        [Authorize]
        [HttpGet("user")]
        public List<MessageModel> Index(string? from, string? to)
        {
            return _messageService.GetMessages(from ?? "", to ?? "");
        }

        [Authorize]
        [HttpGet("group")]
        public List<MessageModel> Group(string groupId)
        {
            return _messageService.GetGroupMessages(groupId);
        }
    }
}
