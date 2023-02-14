using Dapper;
using Dapper.Contrib.Extensions;
using HSS.SharedModels.Entities;
using HSS.SharedModels.Models;
using HSS.SharedServices.Friends;
using HSS.SharedServices.Friends.Contracts;
using HSS.SharedServices.Friends.Services;
using Microsoft.Data.SqlClient;

namespace HSS.SharedServices.Sql.Friends
{
    public class FriendService : IFriendService
    {
        private readonly string connectionString;

        public FriendService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task AddFriendAsync(AddFriendRequest request)
        {
            using var connnection = new SqlConnection(connectionString);
            SqlMapperExtensions.TableNameMapper = (type) =>
            {
                return "hss.Friends";
            };

            var existsIds = await connnection.QueryAsync<string>("SELECT FriendId FROM hss.Friends WHERE UserId=@userId AND FriendId=@friendId", new
            {
                userId = request.UserId,
                friendId = request.TargetUserId
            });

            if (!existsIds.Any())
            {
                await connnection.InsertAsync(new FriendEntity()
                {
                    UserId = request.UserId,
                    FriendId = request.TargetUserId
                });
            }
        }

        public IList<FriendModel> GetFriends(string userId)
        {
            using (var connnection = new SqlConnection(connectionString))
            {
                var friendIds = connnection.Query<string>("SELECT FriendId FROM hss.Friends WHERE UserId=@userId", new { userId });
                var results = connnection.Query<FriendModel>("SELECT Id as UserId, Email, NickName, AvatarUrl, TenantId FROM dbo.AspNetUsers WHERE Id IN @ids", new { ids = friendIds });
                return results.ToList();
            }
        }

        public IList<UserModel> GetUsers(IEnumerable<string> userIds)
        {
            using (var connnection = new SqlConnection(connectionString))
            {
                var results = connnection.Query<UserModel>("SELECT Id as UserId, Email, NickName, AvatarUrl, TenantId FROM dbo.AspNetUsers WHERE Id IN @ids", new { ids = userIds });
                return results.ToList();
            }
        }
    }
}
