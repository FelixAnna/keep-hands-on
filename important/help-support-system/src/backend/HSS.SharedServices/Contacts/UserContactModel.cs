using HSS.SharedServices.Friends;
using HSS.SharedServices.Groups;

namespace HSS.SharedServices.Contacts
{
    public class UserContactModel
    {
        public IEnumerable<GroupModel> Groups { get; set; } = null!;

        public IEnumerable<FriendModel> Friends { get; set; } = null!;
    }
}