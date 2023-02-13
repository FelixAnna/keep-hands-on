using HSS.SharedModels.Models;

namespace HSS.SharedServices.Contacts
{
    public class UserColleagueModel
    {
        public string UserId { get; set; } = null!;
        public IEnumerable<ColleagueModel> Colleagues { get; set; } = null!;
    }

    public class ColleagueModel : UserModel
    {
        public bool IsFriend { get; set; }
    }
}