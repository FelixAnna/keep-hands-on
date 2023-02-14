using Dapper;
using HSS.SharedServices.Contacts;
using HSS.SharedServices.Contacts.Contracts;
using HSS.SharedServices.Contacts.Services;
using HSS.SharedServices.Friends.Services;
using HSS.SharedServices.Groups.Services;
using HSS.SharedServices.Messages;
using HSS.SharedServices.Sql.Messages;
using Microsoft.Data.SqlClient;
using System.Security.Claims;

namespace HSS.SharedServices.Sql.Contact
{
    public class ContactService : IContactService
    {
        private readonly string connectionString;
        private readonly IFriendService friendService;
        private readonly IMessageService messageService;
        private readonly IGroupService groupService;

        public ContactService(IGroupService groupService, IFriendService friendService, IMessageService messageService, string connectionString)
        {
            this.groupService = groupService;
            this.friendService = friendService;
            this.messageService = messageService;
            this.connectionString = connectionString;
        }

        public GetContactResponse GetUserContact(string userId)
        {
            var groups = groupService.GetUserGroups(userId);
            var friends = friendService.GetFriends(userId);

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

        public GetContactResponse GetHistoricalContact(string userId)
        {
            var messengers = messageService.GetMessengers(userId);
            var messengerIds = messengers.SelectMany(x =>
            {
                return new string[] { x.From, x.To };
            })
                .Distinct()
                .Where(x => x != userId)
                .ToArray();

            var groups = groupService.GetUserGroups(userId);
            var friends = friendService.GetUsers(messengerIds);

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
        public GetColleagueResponse GetColleagues(string userId, string keywords)
        {

            var friends = friendService.GetFriends(userId);
            keywords = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(keywords)!;
            var colleagues = GetUserColleagues(userId, keywords);

            foreach (var item in colleagues)
            {
                item.IsFriend = friends.Any(y => y.UserId == item.UserId);
            }

            var results = new GetColleagueResponse()
            {
                UserId = userId,
                Colleagues = colleagues
            };

            return results;
        }

        private IList<ColleagueModel> GetUserColleagues(string userId, string keywords)
        {
            using (var connnection = new SqlConnection(connectionString))
            {
                var currentUserTenantId = connnection.Query<string>("SELECT TenantId FROM dbo.AspNetUsers WHERE Id=@userId", new { userId });
                var results = connnection.Query<ColleagueModel>("SELECT Id as UserId, Email, NickName, AvatarUrl, TenantId FROM dbo.AspNetUsers WHERE TenantId = @tenantId AND IsAdhoc=0", new { tenantId = currentUserTenantId.First() });
                if (!string.IsNullOrWhiteSpace(keywords))
                {
                    results = results.Where(x => x.Email.Contains(keywords, StringComparison.OrdinalIgnoreCase) || x.NickName.Contains(keywords, StringComparison.OrdinalIgnoreCase));
                }

                return results.ToList();
            }
        }
    }
}
