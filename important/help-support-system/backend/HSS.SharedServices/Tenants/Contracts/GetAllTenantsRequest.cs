using HSS.Common.Entities;

namespace HSS.SharedServices.Tenants.Contracts;

public class GetAllTenantsRequest
{
    public TenantStatus Status { get; set; }
}