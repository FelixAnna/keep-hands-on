using System.ComponentModel.DataAnnotations;

namespace HSS.IdentityServer.Models;

public class TenantModel
{
    public int Id { get; set; }

    [Required]
    [DataType(DataType.Text)]
    [Display(Name = "Tenant Name")]
    public string Name { get; set; } = null!;
}