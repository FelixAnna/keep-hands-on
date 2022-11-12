using IdentityServer4.Models;
using IdentityServer4;

namespace HSS.IdentityServer
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
                    AllowedScopes = new List<string> { "read" },
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
                    AllowedScopes = new List<string> { "read", "write" },
                    RequirePkce = false,
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

        public static IList<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("externalApi", "External API")
                {
                    Scopes = { "read", "write", "delete" }
                },

                new ApiResource("invoice", "Invoice API")
                {
                    Scopes = { "invoice.read", "invoice.pay", "manage" }
                },

                new ApiResource("customer", "Customer API")
                {
                    Scopes = { "customer.read", "customer.contact", "manage" }
                }
            };
        }
    }
}
