using System.ComponentModel.DataAnnotations.Schema;

namespace HSS.SharedModels.Entities;

[Table("Tenants", Schema = "hss")]
public class TenantEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public TenantStatus Status { get; set; }
}

public enum TenantStatus
{
    New = 1,
    Approved = 2,
    Ready = 256
}