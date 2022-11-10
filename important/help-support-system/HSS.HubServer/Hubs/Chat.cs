using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace HSS.HubServer
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class Chat : Hub
    {
        public override Task OnConnectedAsync()
        {
            var userName = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Groups.AddToGroupAsync(Context.ConnectionId, userName);
            return base.OnConnectedAsync();
        }

        public async Task SendMessage(string toUser, string message)
        {
            var userName = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await Clients.All.SendAsync("ReceiveMessage", userName, message);
        }

        public async Task SendToUser(string toUser, string message)
        {
            var sender = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await Clients.Group(toUser).SendAsync("ReceiveMessage", sender, message);
        }
    }
}
