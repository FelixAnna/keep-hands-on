using Dapper;
using HSS.SharedServices.Contacts;
using HSS.SharedServices.Contacts.Contracts;
using HSS.SharedServices.Contacts.Services;
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
                var results = connnection.Query<FriendModel>("SELECT Id as UserId, Email, UserName FROM dbo.AspNetUsers WHERE Id IN @ids", new { ids = friendIds });
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
    }
}
