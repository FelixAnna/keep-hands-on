using EStore.Common;
using EStore.UserApi.Users.Contracts;
using EStore.UserApi.Users.Services;

namespace EStore.ProductAPI.Extensions
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {

            // Get values from the config given their key and their target type.
            IdentityPlatformSettings settings = configuration.GetRequiredSection("IdentityPlatformSettings").Get<IdentityPlatformSettings>()!;

            var connectionString = configuration["ConnectionStrings:DefaultConnection"]!;
            services.AddSingleton(settings);
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
