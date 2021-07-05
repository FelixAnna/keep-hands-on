using Grpc.Core;
using Pocker;
using PokerServer.Poker;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokerServer.ApplicationServices
{
    public class RoomService : IRoomService
    {
        private const int RoomCapacity = 2;
        private readonly ConcurrentDictionary<string, IServerStreamWriter<StreamResponse>> users = new();
        private readonly IPokerService pokerService;
        public RoomService(IPokerService pokerService)
        {
            this.pokerService = pokerService;
        }

        public async Task JoinAsync(string name, int type, IServerStreamWriter<StreamResponse> response)
        {
            //join
            if (users.Count >= RoomCapacity)
            {
                throw new Exception("Room is full.");
            }
            else
            {
                users.Remove(name, out _);
                users.TryAdd(name, response);

                //connect
                await BroadcastToAllAsync("system", $"user {name} connected!");
                if (users.Count == RoomCapacity)
                {
                    pokerService.Initial();
                    await BroadcastToAllAsync("system", $"Ready to play!", pokerService.Print());
                }
            }
        }

        public async Task Remove(string name)
        {
            await BroadcastToAllAsync("system", $"user {name} exit!");
            users.TryRemove(name, out var _);
        }

        public async Task BroadcastMessageAsync(StreamRequest request) => await BroadcastMessages(request);

        private async Task BroadcastMessages(StreamRequest request)
        {
            if (pokerService.IsFinished())
            {
                //finished
                await BroadcastToOneAsync(request.Username, "system", $"Game already over!");
                return;
            }

            if (users.Count != RoomCapacity)
            {
                //not full, wait
                await BroadcastToOneAsync(request.Username, "system", $"Please wait for others join");
                return;
            }

            if (!pokerService.Pick(request.Username, request.Row, request.Count))
            {
                //failed command
                await BroadcastToOneAsync(request.Username, "system", $"Invalid command, either this is not your turn or the input is invalid!");
                return;
            }

            var message = $"picked {request.Count} poker(s) from line {request.Row}";
            var remaining = pokerService.Print();
            await BroadcastToAllAsync(request.Username, message, remaining);

            var winner = pokerService.GetWinner();
            if (!string.IsNullOrEmpty(winner))
            {
                //broadcast winner
               await BroadcastToAllAsync("system", $"Game over: {winner} win!");
            }
        }

        private async Task BroadcastToOneAsync(string targetUser, string userName, string message, string remaining = null)
        {
            var user = users.First(x => targetUser == x.Key);
            await SendMessageToSubscriber(user, userName, message, remaining);
        }

        private async Task BroadcastToAllAsync(string userName, string message, string remaining = null)
        {
            foreach (var user in users)
            {
                await SendMessageToSubscriber(user, userName, message, remaining);
            }
        }

        private static async Task SendMessageToSubscriber(KeyValuePair<string, IServerStreamWriter<StreamResponse>> user, string userName, string message, string remaining = null)
        {
            var response = new StreamResponse()
            {
                Username = userName,
                Message = message,
                Remaining = remaining ?? string.Empty
            };

            await user.Value.WriteAsync(response);
        }
    }
}
