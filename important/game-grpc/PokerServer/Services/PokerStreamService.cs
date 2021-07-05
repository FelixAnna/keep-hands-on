using Grpc.Core;
using Microsoft.Extensions.Logging;
using Pocker;
using PokerServer.ApplicationServices;
using System;
using System.Threading.Tasks;

namespace PokerServer.Services
{
    public class PokerStreamService : Pocker.StreamService.StreamServiceBase
    {
        private readonly IRoomService _roomService;

        private ILogger<PokerStreamService> _logger;
        public PokerStreamService(ILogger<PokerStreamService> logger, IRoomService roomService)
        {
            _logger = logger;
            _roomService = roomService;
        }

        public override async Task ConnectAsync(IAsyncStreamReader<StreamRequest> requestStream, IServerStreamWriter<StreamResponse> responseStream, ServerCallContext context)
        {
            if (!await requestStream.MoveNext())
                return;

            try
            {
                do
                {
                    if (requestStream.Current.Type == 1) //join
                    {
                        await _roomService.JoinAsync(requestStream.Current.Username, requestStream.Current.Type, responseStream);
                    }
                    else if (requestStream.Current.Type == 255) //exit
                    {
                        await _roomService.Remove(requestStream.Current.Username);
                    }
                    else
                    {
                        await _roomService.BroadcastMessageAsync(requestStream.Current);
                    }
                } while (await requestStream.MoveNext());
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
            }
        }
    }
}
