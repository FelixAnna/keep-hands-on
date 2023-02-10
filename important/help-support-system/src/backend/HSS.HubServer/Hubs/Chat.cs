using HSS.SharedServices.Contacts.Services;
using HSS.SharedServices.Messages;
using HSS.SharedServices.Messages.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace HSS.HubServer
{
    [Authorize(Policy = "Customer")]
    public class Chat : Hub
    {
        private readonly IContactService contactService;
        private readonly IMessageService messageService;

        public Chat(IContactService contactService, IMessageService messageService)
        {
            this.contactService = contactService;
            this.messageService = messageService;
        }

        public override Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var contacts = contactService.GetUserContact(userId);
            Groups.AddToGroupAsync(Context.ConnectionId, userId);
            foreach (var group in contacts.Contact.Groups)
            {
                Groups.AddToGroupAsync(Context.ConnectionId, group.GroupId.ToString());
            }

            return base.OnConnectedAsync();
        }

        public async Task SendToUser(string toUser, string message)
        {
            var sender = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            var msg = new SaveMessageRequest
            {
                Sender = sender,
                Receiver = toUser,
                Content = message
            };
            await messageService.SaveMessageAsync(msg);

            await Clients.Group(toUser).SendAsync("ReceiveMessage", sender, toUser, message);
        }

        public async Task SendToGroup(string toGroup, string message)
        {
            var sender = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            var msg = new SaveMessageRequest
            {
                Sender = sender,
                Receiver = toGroup,
                Content = message
            };
            await messageService.SaveMessageAsync(msg);

            await Clients.Group(toGroup).SendAsync("ReceiveGroupMessage", sender, toGroup, message);
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var contacts = contactService.GetUserContact(userId);
            Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
            foreach (var group in contacts.Contact.Groups)
            {
                Groups.RemoveFromGroupAsync(Context.ConnectionId, group.GroupId.ToString());
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}
