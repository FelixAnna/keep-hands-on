using HSS.SharedServices.Friends.Contracts;

namespace HSS.SharedServices.Friends.Services
{
    public interface IFriendService
    {
        IList<FriendModel> GetFriends(string userId);

        Task AddFriendAsync(AddFriendRequest request);
    }
}
