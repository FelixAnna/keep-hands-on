using Dapper;
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
                if(groupId != newGroupId.FirstOrDefault())
                {
                    throw new InvalidOperationException($"User with id {userId} not in group with id {groupId}");
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
                var groupMembers = connnection.Query<GroupMember>("SELECT Id as UserId, UserName, Email FROM dbo.AspNetUsers WHERE Id IN @ids", new { ids = groupMemberIds });

                return groupMembers.ToList();
            }
        }
    }
}
