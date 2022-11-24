using IdentityModel;
using IdentityServer4;
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
                    AllowedScopes = new List<string> { "read", "openid", "profile", "HSS.IdentityServerAPI" },
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
                    AllowedScopes = new List<string> { "read", "write", "openid", "profile", "HSS.IdentityServerAPI" },
                    RequirePkce = false,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RedirectUris = new[] { "https://localhost:7075/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:7075/signout-oidc" }
                },

                new Client()
                {
                    ClientId = "webclient3",
                    ClientName = "Example client application using client credentials or code",
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    ClientSecrets = new List<Secret> { new Secret("SomethingUnknown".Sha256()) }, // change me!
                    AllowedScopes = new List<string> { "read" },
                    RequirePkce = false,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RedirectUris = new[] { "https://localhost:7076/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:7076/signout-oidc" }
                },

                new Client
                {
                    ClientId = "webclient4",
                    ClientName = "SignalR",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    ClientSecrets = new List<Secret> { new Secret("SomethingUnknown".Sha256()) }, // change me!

                    RedirectUris = { "http://localhost:7133/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:7133/signout-callback-oidc" },
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "read"
                    },
                    AllowOfflineAccess = true
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
                new ApiResource("hub", "SignalR Hub API")
                {
                    Scopes = { "hub.read", "hub.write" }
                },

                new ApiResource("user", "User API")
                {
                    Scopes = { "user.read", "user.contact" }
                }
            };
        }
    }
}
