using System.ComponentModel.DataAnnotations.Schema;

namespace HSS.Common.Entities;

[Table("Tenants", Schema ="hss")]
public class TenantEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public TenantStatus Status { get; set; }
}

public enum TenantStatus {
    New,
    Approved,
    Ready=255
}