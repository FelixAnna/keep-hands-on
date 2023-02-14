using IdentityServer4.Models;

namespace HSS.IdentityServer.IdentityConfigurations
{
    public class IdentityServerConfiguration
    {
        public static IList<Client> GetClients()
        {
            return new List<Client>
            {
                new Client()
                {
                    ClientId = "webclient",
                    ClientName = "Example client application using client credentials or code",
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    ClientSecrets = new List<Secret> { new Secret("SomethingUnknown".Sha256()) }, // change me!
                    AllowedScopes = new List<string> { "read", "openid", "profile", "HSS.IdentityServerAPI", "user.admin", "message.admin", "message.readwrite", "user.profile", "user.contact" },
                    RequirePkce = false,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RedirectUris = new[] { "https://localhost:7075/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:7075/signout-oidc" }
                },

                new Client()
                {
                    ClientId = "webclient2",
                    ClientName = "Example client application using passowrd",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = new List<Secret> { new Secret("SomethingUnknown".Sha256()) }, // change me!
                    AllowedScopes = new List<string> { "read", "write", "openid", "profile", "HSS.IdentityServerAPI", "message.readwrite", "user.profile", "user.contact" },
                    RequirePkce = false,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RedirectUris = new[] { "https://localhost:7075/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:7075/signout-oidc" }
                }
            };
        }

        public static IList<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
                new IdentityResources.Phone(),
                new IdentityResources.Address(),
            };
        }

        public static IList<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("message", "Hub Message API")
                {
                    Scopes = { "message.readwrite", "message.admin" }
                },

                new ApiResource("user", "User API")
                {
                    Scopes = { "user.profile", "user.contact" }
                }
            };
        }
    }
}
