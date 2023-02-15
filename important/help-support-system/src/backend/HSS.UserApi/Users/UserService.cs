using HSS.Common;
using HSS.Common.Exceptions;
using HSS.SharedModels.Models;
using HSS.SharedServices.Tenants.Services;
using HSS.UserApi.Users.Services;
using IdentityModel;
using IdentityModel.Client;
using System.IdentityModel.Tokens.Jwt;

namespace HSS.UserApi.Users.Contracts
{

    public class UserService : IUserService
    {
        private readonly HttpClient httpClient;
        private readonly DiscoveryDocumentResponse disco;
        private readonly IdentityPlatformSettings settings;
        private readonly IConsulHttpClient client;
        private readonly ITenantService tenantService;

        public UserService(IdentityPlatformSettings settings, IConsulHttpClient client, ITenantService tenantService)
        {
            httpClient = new HttpClient();
            disco = httpClient.GetDiscoveryDocumentAsync(settings.Authority).Result;
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
            }

            Console.WriteLine("TokenEndpoint: {0}", disco.TokenEndpoint);

            this.settings = settings;
            this.client = client;
            this.tenantService = tenantService;
        }

        public async Task<LoginResponse> PasswordSignInAsync(LoginRequest request)
        {
            var identityServerResponse = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                GrantType = "password",

                ClientId = settings.ClientId,
                ClientSecret = settings.ClientSecret,
                Scope = "openid profile message.readwrite user.profile user.contact",

                UserName = request.UserName,
                Password = request.Password,
            });

            if (!identityServerResponse.IsError)
            {
                Console.WriteLine("User: {0}", identityServerResponse.Json);

                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(identityServerResponse.AccessToken);
                return new LoginResponse
                {
                    AccessToken = identityServerResponse.AccessToken,
                    Profile = new UserModel
                    {
                        UserId = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject)?.Value!,
                        NickName = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.NickName)?.Value!,
                        TenantId = int.Parse(jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "tenantId")?.Value!),
                        AvatarUrl = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Picture)?.Value!,
                        Email = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Email)?.Value!
                    }
                };
            }

            throw new HSSOperationFailedException(identityServerResponse.Error);
        }

        public async Task<FakeLoginResponse> FakeRegisterAsync(int tenantId)
        {
            var request = new FakeAuthInputModel()
            {
                TenantId = tenantId,
                AvatarUrl = "http://media.bizj.us/view/img/7182082/thinkstockphotos-452415785*750xx2122-1194-0-111.jpg",
                NickName = "Customer",
                Password = "123456aA@",
            };

            UserModel user;
            int count = 0;
            do
            {
                request.Email = DateTime.Now.Ticks + "@adhoc.com";
                user = await client.PostAsync<UserModel, FakeAuthInputModel>("Idp Service", "fakeauth/signup", request);
            } while (string.IsNullOrEmpty(user?.UserId) && count++ < 3);

            if (string.IsNullOrEmpty(user?.UserId))
            {
                throw new HSSOperationFailedException("Failed to register adhoc user");
            }

            var support = await tenantService.GetSupportAsync(tenantId);

            var response = await PasswordSignInAsync(new LoginRequest()
            {
                UserName = request.Email,
                Password = request.Password
            });
            return new FakeLoginResponse()
            {
                AccessToken = response.AccessToken,
                Profile = response.Profile,
                SupportProfile = support,
            };
        }
    }
}
