using Dapper;
using HSS.SharedModels.Models;
using HSS.SharedServices.Tenants;
using HSS.SharedServices.Tenants.Contracts;
using HSS.SharedServices.Tenants.Services;
using Microsoft.Data.SqlClient;

namespace HSS.SharedServices.Sql.Tenants
{
    public class TenantService : ITenantService
    {
        private readonly string connectionString;

        public TenantService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<IList<TenantModel>> GetAllTenantsAsync(GetAllTenantsRequest request)
        {
            using var connnection = new SqlConnection(connectionString);
            var builder = new SqlBuilder();

            var select = builder.AddTemplate("select * from hss.Tenants");
            var parameter = new DynamicParameters();

            parameter.Add("status", request.Status);
            builder.Where(" [Status] = @status ");

            var results = await connnection.QueryAsync<TenantModel>(select.RawSql, parameter);

            return results.ToList();
        }

        public async Task<UserModel> GetSupportAsync(int tenantId)
        {
            using var connnection = new SqlConnection(connectionString);
            var builder = new SqlBuilder();

            var select = builder.AddTemplate(@"SELECT TOP 1 u.Id as UserId, u.Email, u.NickName, u.AvatarUrl, u.TenantId 
FROM [dbo].[AspNetUsers] u
inner join [dbo].[AspNetUserRoles] ur on u.Id = ur.UserId
inner join [dbo].[AspNetRoles] r on r.Id = ur.RoleId
WHERE ISAdHoc=0 and TenantId=@tenantId and r.Name='Support'");
            var parameter = new DynamicParameters();

            parameter.Add("tenantId", tenantId);

            var results = await connnection.QueryFirstAsync<UserModel>(select.RawSql, parameter);

            return results;
        }
    }
}