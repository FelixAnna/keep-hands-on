using Microsoft.AspNetCore.Identity;

namespace EStore.IdentityServer.Models;

public class ApplicationUser : IdentityUser
{
    [PersonalData]
    public string? NickName { get; set; }
    [PersonalData]
    public string? AvatarUrl { get; set; }
}
