using Grpc.Core;
using Pocker;
using System.Threading.Tasks;

namespace PokerServer.ApplicationServices
{
    public interface IRoomService
    {
        Task BroadcastMessageAsync(StreamRequest request);
        Task JoinAsync(string name, int type, IServerStreamWriter<StreamResponse> response);
        Task Remove(string name);
    }
}