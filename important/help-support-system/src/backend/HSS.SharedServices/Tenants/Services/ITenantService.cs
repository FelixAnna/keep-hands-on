using HSS.SharedServices.Tenants.Contracts;

namespace HSS.SharedServices.Tenants.Services
{
    public interface ITenantService
    {
        Task<IList<TenantModel>> GetAllTenantsAsync(GetAllTenantsRequest request);
    }
}