using HSS.SharedModels.Models;
using HSS.SharedServices.Groups;

namespace HSS.SharedServices.Contacts
{
    public class UserContactModel
    {
        public IEnumerable<GroupModel> Groups { get; set; } = null!;

        public IEnumerable<UserModel> Friends { get; set; } = null!;
    }
}