using Grpc.Core;
using Grpc.Net.Client;
using Pocker;
using System;
using System.Threading.Tasks;

namespace PokerClientApp
{
    public class ConnectService
    {
        private string _name;
        private AsyncDuplexStreamingCall<StreamRequest, StreamResponse> _stream;
        private Task _responseTask;
        
        public async Task ConnectAsync(string name, Action<StreamResponse> callBack)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");

            var client = new StreamService.StreamServiceClient(channel);
            _name = name;
            _stream = client.ConnectAsync();
            _responseTask = Task.Run(async () =>
            {
                await foreach (var rm in _stream.ResponseStream.ReadAllAsync())
                {
                    callBack(rm);
                }
            });

            await _stream.RequestStream.WriteAsync(new StreamRequest { Username = _name, Type =1});
        }

        public async Task SendAsync(int row, int count)
        {
            await _stream.RequestStream.WriteAsync(new StreamRequest
            {
                Username = _name,
                Row = row,
                Count = count,
                Type=2
            });
        }

        public async Task DisconnectAsync()
        {
            await _stream.RequestStream.WriteAsync(new StreamRequest { Username = _name, Type = 255 });

            await _stream.RequestStream.CompleteAsync();
            await _responseTask;
        }

        public void Dipose()
        {
            _stream.Dispose();
        }
    }
}
