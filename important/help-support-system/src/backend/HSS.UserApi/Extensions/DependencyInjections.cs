using HSS.Common;
using HSS.SharedServices.Contacts.Services;
using HSS.SharedServices.Friends.Services;
using HSS.SharedServices.Groups.Services;
using HSS.SharedServices.Messages;
using HSS.SharedServices.Sql.Contact;
using HSS.SharedServices.Sql.Friends;
using HSS.SharedServices.Sql.Messages;
using HSS.SharedServices.Sql.Tenants;
using HSS.SharedServices.Tenants.Services;
using HSS.UserApi.Users.Contracts;
using HSS.UserApi.Users.Services;

namespace HSS.UserApi.Extensions
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Get values from the config given their key and their target type.
            IdentityPlatformSettings settings = configuration.GetRequiredSection("IdentityPlatformSettings").Get<IdentityPlatformSettings>();

            var connectionString = configuration.GetValue<string>("hss:sqlconn");
            services.AddSingleton(settings);
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGroupService, GroupService>(_ => new GroupService(connectionString));
            services.AddScoped<IContactService, ContactService>(x => new ContactService(x.GetRequiredService<IGroupService>(), x.GetRequiredService<IFriendService>(), x.GetRequiredService<IMessageService>(), connectionString));
            services.AddScoped<ITenantService, TenantService>(_ => new TenantService(connectionString));
            services.AddScoped<IFriendService, FriendService>(provider => new FriendService(connectionString));
            services.AddScoped<IMessageService, MessageService>(provider => new MessageService(connectionString));
            services.AddSingleton<IConsulHttpClient, ConsulHttpClient>();
            services.AddSingleton<HttpClient>();
            return services;
        }
    }
}
