using HSS.SharedServices.Groups.Contracts;

namespace HSS.SharedServices.Groups.Services
{
    public interface IGroupService
    {
        GetGroupMemebersResponse GetGroupMembers(string userId, string groupId);

        IList<GroupModel> GetUserGroups(string userId);
    }
}
