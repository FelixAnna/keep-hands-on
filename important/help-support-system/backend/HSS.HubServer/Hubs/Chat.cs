using HSS.HubServer.EFCoreGen;
using HSS.HubServer.EFCoreGen.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace HSS.HubServer
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class Chat : Hub
    {
        SshDbContext _sshDbContext;
        public Chat(SshDbContext sshDbContext)
        {
            _sshDbContext = sshDbContext;
        }
        public override Task OnConnectedAsync()
        {
            var userName = Context.User?.FindFirst(ClaimTypes.Email)?.Value;

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
            var sender = Context.User?.FindFirst(ClaimTypes.Email)?.Value;
            
            await _sshDbContext.Messages.AddAsync(new Message
            {
                From = sender,
                To = toUser,
                Content = message
            });
            _sshDbContext.SaveChanges();

            await Clients.Group(toUser).SendAsync("ReceiveMessage", sender, message);
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _sshDbContext.Dispose();
            return base.OnDisconnectedAsync(exception);
        }
    }
}
