using HSS.SharedModels.Models;
using HSS.SharedServices.Tenants.Contracts;

namespace HSS.SharedServices.Tenants.Services
{
    public interface ITenantService
    {
        Task<IList<TenantModel>> GetAllTenantsAsync(GetAllTenantsRequest request);

        Task<UserModel> GetSupportAsync(int tenantId);
    }
}