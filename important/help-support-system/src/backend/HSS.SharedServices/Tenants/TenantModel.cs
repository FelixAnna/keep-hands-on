using HSS.SharedModels.Entities;

namespace HSS.SharedServices.Tenants
{
    public class TenantModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public TenantStatus Status { get; set; }
    }
}