using Microsoft.AspNetCore.Identity;

namespace HSS.IdentityServer.Models;

public class ApplicationUser : IdentityUser
{
    [PersonalData]
    public string? NickName { get; set; }
    [PersonalData]
    public string? AvatarUrl { get; set; }

    [PersonalData]
    public int? TenantId { get; set; }

    public bool IsAdHoc { get; set; }
}
