using HSS.IdentityServer.Models;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace HSS.IdentityServer.IdentityConfigurations
{
    public class ProfileService : IProfileService
    {
        protected UserManager<ApplicationUser> _userManager;

        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);

            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Email, user.Email),
                new Claim(JwtClaimTypes.Name, user.NickName!),
                new Claim(JwtClaimTypes.NickName, user.NickName!),
                new Claim("tenantId", user.TenantId!.ToString()??""),
                new Claim(JwtClaimTypes.Picture, user.AvatarUrl!),
            };

            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);

            context.IsActive = user != null;
        }
    }
}
