using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EStore.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConsulConfig(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var consulCondig = configuration.GetRequiredSection("ConsulConfig").Get<ConsulConfig>();

            services.AddSingleton(consulCondig);

            services.AddSingleton<IConsulClient>(_ => new ConsulClient(consulConfig =>
            {
                consulConfig.Address = new Uri(consulCondig.Address);
            }));

            return services;
        }
    }
}
