using HSS.SharedServices.Messages;
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

        [Authorize(Policy = "Customer")]
        [HttpGet("user")]
        public List<MessageModel> Index(string? from, string? to)
        {
            return _messageService.GetMessages(from ?? "", to ?? "");
        }

        [Authorize(Policy = "Customer")]
        [HttpGet("group")]
        public List<MessageModel> Group(string groupId)
        {
            //ensure user in group or have permission
            //to do

            return _messageService.GetGroupMessages(groupId);
        }
    }
}
