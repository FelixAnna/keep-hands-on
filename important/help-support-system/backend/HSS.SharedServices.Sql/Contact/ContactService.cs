using Dapper;
using HSS.SharedServices.Contact;
using HSS.SharedServices.Contact.Contracts;
using HSS.SharedServices.Contact.Services;
using Microsoft.Data.SqlClient;

namespace HSS.SharedServices.Sql.Contact
{
    public class ContactService : IContactService
    {
        private readonly string connectionString;

        public ContactService(string connectionString)
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

        public GetContactResponse GetUserContact(string userId)
        {
            var groups = GetUserGroups(userId);
            var friends = GetUserFriends(userId);

            return new GetContactResponse()
            {
                UserId = userId,

                Contact = new UserContactModel
                {
                    Groups = groups,
                    Friends = friends
                }
            };
        }

        private IList<FriendModel> GetUserFriends(string userId)
        {
            using (var connnection = new SqlConnection(connectionString))
            {
                var friendIds = connnection.Query<string>("SELECT FriendId FROM hss.Friends WHERE UserId=@userId", new { userId });
                var results = connnection.Query<FriendModel>("SELECT Id as UserId, Email, UserName as Name FROM dbo.AspNetUsers WHERE Id IN @ids", new { ids = friendIds });
                return results.ToList();
            }
        }

        private IList<GroupModel> GetUserGroups(string userId)
        {
            using (var connnection = new SqlConnection(connectionString))
            {
                var groupIds = connnection.Query<string>("SELECT GroupId FROM hss.GroupMembers WHERE UserId=@userId", new { userId = userId });
                var results = connnection.Query<GroupModel>("SELECT * FROM hss.Groups WHERE GroupId IN @ids", new { ids = groupIds });

                return results.ToList();
            }
        }

        private GroupModel GetGroup(string groupId, string userId)
        {
            using (var connnection = new SqlConnection(connectionString))
            {
                var newGroupId = connnection.Query<string>("SELECT GroupId FROM hss.GroupMembers WHERE UserId=@userId and GroupId=@groupId", new { userId, groupId });
                if(groupId != newGroupId.FirstOrDefault())
                {
                    throw new InvalidOperationException($"User with id {userId} not in group with id {groupId}");
                }

                var group = connnection.QueryFirst<GroupModel>("SELECT * FROM hss.Groups WHERE GroupId=@groupId ", new { groupId });

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
