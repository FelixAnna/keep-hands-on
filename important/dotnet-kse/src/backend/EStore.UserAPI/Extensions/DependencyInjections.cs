using EStore.Common;
using EStore.UserAPI.Users.Contracts;
using EStore.UserAPI.Users.Services;

namespace EStore.UserAPI.Extensions
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {

            // Get values from the config given their key and their target type.
            IdentityPlatformSettings settings = configuration.GetRequiredSection("IdentityPlatformSettings").Get<IdentityPlatformSettings>()!;

            var connectionString = configuration["kse:sqlconn"]!;
            services.AddSingleton(settings);
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
