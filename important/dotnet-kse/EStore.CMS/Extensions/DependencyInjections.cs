using EStore.Common;
using EStore.DataAccess.MemCache.Products;
using EStore.DataAccess.Wrapper.Products;
using EStore.SharedServices.Products.Repositories;
using EStore.SharedServices.Products.Services;
using EStore.SharedServices.SqlServer.Products;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using NuGet.Configuration;
using System.Security.Claims;

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
            services.AddMemoryCache();
            services.AddScoped(op => new SqlProductRepository(connectionString));
            services.AddScoped<CachedProductRepository>();
            services.AddScoped<IProductRepository, WrapperProductRepository>();
            services.AddScoped<IProductService, ProductService>();

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
                    options.Scope.Add("hub.write");
                    options.Scope.Add("hub.read");
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

            services.AddAuthorization(options =>
            {
                options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireClaim(ClaimTypes.NameIdentifier);
                });
            });
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
