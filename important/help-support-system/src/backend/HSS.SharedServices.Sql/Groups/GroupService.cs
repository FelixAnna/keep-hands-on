using Dapper;
using HSS.Common.Exceptions;
using HSS.SharedServices.Groups;
using HSS.SharedServices.Groups.Contracts;
using HSS.SharedServices.Groups.Services;
using Microsoft.Data.SqlClient;

namespace HSS.SharedServices.Sql.Contact
{
    public class GroupService : IGroupService
    {
        private readonly string connectionString;

        public GroupService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public GetGroupMemebersResponse GetGroupMembers(string userId, string groupId)
        {
            var group = GetGroup(groupId, userId);
            var members = GetGroupMembers(groupId);

            return new GetGroupMemebersResponse
            {
                Group = group,
                Memebers = members.ToList()
            };
        }

        private Group GetGroup(string groupId, string userId)
        {
            using (var connnection = new SqlConnection(connectionString))
            {
                var newGroupId = connnection.Query<string>("SELECT GroupId FROM hss.GroupMembers WHERE UserId=@userId and GroupId=@groupId", new { userId, groupId });
                if (groupId != newGroupId.FirstOrDefault())
                {
                    throw new HSSInvalidOperationException($"User with id {userId} not in group with id {groupId}");
                }

                var group = connnection.QueryFirst<Group>("SELECT * FROM hss.Groups WHERE GroupId=@groupId ", new { groupId });

                return group;
            }
        }

        private IList<GroupMember> GetGroupMembers(string groupId)
        {
            using (var connnection = new SqlConnection(connectionString))
            {
                var groupMemberIds = connnection.Query<string>("SELECT UserId FROM hss.GroupMembers WHERE GroupId=@groupId", new { groupId });
                var groupMembers = connnection.Query<GroupMember>("SELECT Id as UserId, NickName, AvatarUrl, Email, TenantId FROM dbo.AspNetUsers WHERE Id IN @ids", new { ids = groupMemberIds });

                return groupMembers.ToList();
            }
        }

        public IList<GroupModel> GetUserGroups(string userId)
        {
            using (var connnection = new SqlConnection(connectionString))
            {
                var groupIds = connnection.Query<string>("SELECT GroupId FROM hss.GroupMembers WHERE UserId=@userId", new { userId = userId });
                var results = connnection.Query<GroupModel>("SELECT * FROM hss.Groups WHERE GroupId IN @ids", new { ids = groupIds });

                return results.ToList();
            }
        }
    }
}
