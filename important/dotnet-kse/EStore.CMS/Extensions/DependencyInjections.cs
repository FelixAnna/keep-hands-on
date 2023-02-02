﻿using EStore.Common;
using EStore.Common.Extensions;
using Microsoft.AspNetCore.HttpOverrides;

namespace EStore.CMS.Extensions
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {

            // Get values from the config given their key and their target type.
            IdentityPlatformSettings settings = configuration.GetRequiredSection("IdentityPlatformSettings").Get<IdentityPlatformSettings>()!;

            var connectionString = configuration["ConnectionStrings:DefaultConnection"]!;
            services.AddSingleton(settings); 

            return services;
        }

        public static IServiceCollection AddOpenIdConnect(this IServiceCollection services)
        {
            var settings = services.BuildServiceProvider().GetService<IdentityPlatformSettings>()!;
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "cookie";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("cookie")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = settings.Authority;
                    options.ClientId = settings.ClientId;
                    options.ClientSecret = settings.ClientSecret;
                    options.ResponseType = "code";
                    options.UsePkce = false;
                    options.ResponseMode = "query";
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("product.write"); 
                    options.Scope.Add("product.read");
                    options.Scope.Add("order.admin");
                    options.Scope.Add("cart.admin");
                    options.SaveTokens = true;
                })
                .AddJwtBearer(options =>
                {
                    // Configure the Authority to the expected value for
                    // the authentication provider. This ensures the token
                    // is appropriately validated.
                    // automic validate
                    options.Audience = settings.Audience;
                    options.Authority = settings.Authority;
                    options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
                });

            services.AddEStoreAuthorization();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                // These three subnets encapsulate the applicable Azure subnets. At the moment, it's not possible to narrow it down further.
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            }); 
            
            return services;
        }
    }
}
