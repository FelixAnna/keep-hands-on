using HSS.Common;
using HSS.SharedServices.Messages;
using HSS.SharedServices.Sql.Messages;

namespace HSS.MessageApi.Extensions
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Get values from the config given their key and their target type.
            IdentityPlatformSettings settings = configuration.GetRequiredSection("IdentityPlatformSettings").Get<IdentityPlatformSettings>();

            var connectionString = configuration.GetValue<string>("hss:sqlconn");
            services.AddSingleton(settings);
            services.AddScoped<IMessageService, MessageService>(_ => new MessageService(connectionString));
            return services;
        }

    }
}
