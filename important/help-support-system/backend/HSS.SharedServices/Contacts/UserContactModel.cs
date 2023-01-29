using HSS.Common.Models;

namespace HSS.SharedServices.Contacts
{
    public class UserContactModel
    {
        public IEnumerable<GroupModel> Groups { get; set; } = null!;

        public IEnumerable<FriendModel> Friends { get; set; } = null!;
    }

    public class GroupModel
    {
        public int GroupId { get; set; }
        public string Name { get; set; } = null!;
    }

    public class FriendModel : UserModel
    {
    }
}