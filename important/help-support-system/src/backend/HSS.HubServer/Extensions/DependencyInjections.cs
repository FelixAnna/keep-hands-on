using HSS.Common;
using HSS.SharedServices.Contacts.Services;
using HSS.SharedServices.Friends.Services;
using HSS.SharedServices.Groups.Services;
using HSS.SharedServices.Messages;
using HSS.SharedServices.Sql.Contact;
using HSS.SharedServices.Sql.Messages;

namespace HSS.HubServer.Extensions
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Get values from the config given their key and their target type.
            IdentityPlatformSettings settings = configuration.GetRequiredSection("IdentityPlatformSettings").Get<IdentityPlatformSettings>();

            var connectionString = configuration.GetValue<string>("hss:sqlconn");
            services.AddSingleton(settings);
            services.AddScoped<IContactService, ContactService>(x => new ContactService(x.GetRequiredService<IGroupService>(), x.GetRequiredService<IFriendService>(), connectionString));
            services.AddScoped<IMessageService, MessageService>(provider => new MessageService(connectionString));
            return services;
        }
    }
}
