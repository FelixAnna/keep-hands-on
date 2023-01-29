﻿using HSS.Common;
using HSS.UserApi.Users.Exceptions;
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

        public UserService(IdentityPlatformSettings settings)
        {
            httpClient = new HttpClient();
            disco = httpClient.GetDiscoveryDocumentAsync(settings.Authority).Result;
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
            }

            Console.WriteLine("TokenEndpoint: {0}", disco.TokenEndpoint);

            this.settings = settings;
        }

        public async Task<LoginResponse> PasswordSignInAsync(LoginRequest request)
        {
            var identityServerResponse = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                GrantType = "password",

                ClientId = settings.ClientId,
                ClientSecret = settings.ClientSecret,
                Scope = "openid profile hub.read user.read user.contact",

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
                        TenantId = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "tenantId")?.Value!,
                        AvatarUrl = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Picture)?.Value!,
                        Email = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Email)?.Value!
                    }
                };
            }

            throw new LoginFailedException(identityServerResponse.Error);
        }
    }
}
