using HSS.SharedModels.Models;
using HSS.SharedServices.Friends.Contracts;

namespace HSS.SharedServices.Friends.Services
{
    public interface IFriendService
    {
        IList<FriendModel> GetFriends(string userId);

        IList<UserModel> GetUsers(IEnumerable<string> userIds);

        Task AddFriendAsync(AddFriendRequest request);
    }
}
