using Dapper;
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
    }
}