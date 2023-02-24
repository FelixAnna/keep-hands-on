using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net.Sockets;
using System.Net;

namespace HSS.Common.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder RegisterWithConsul(this IApplicationBuilder app,
         IHostApplicationLifetime lifetime, bool isDevlopment=false)
        {
            // Retrieve Consul client from DI
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var consulConfig = app.ApplicationServices.GetRequiredService<ConsulConfig>();
            // Setup logger
            var logger = app.ApplicationServices.GetRequiredService<ILogger<ConsulConfig>>();

            try
            {
                // Get server IP address
                var features = app.Properties["server.Features"] as FeatureCollection;
                var addresses = features!.Get<IServerAddressesFeature>();
                var address = addresses.Addresses.Last();

                var name = Dns.GetHostName(); // get container id
                var ip = Dns.GetHostEntry(name).AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);

                Console.WriteLine("Container name: " + name + ", Ip is: " 
                    + string.Join(",", Dns.GetHostEntry(name).AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetwork).Select(x=>x.ToString())));
                Console.WriteLine("Service Address is: " + string.Join(",", addresses.Addresses.ToArray()));

                var ipaddress = ip.ToString();
                if (isDevlopment)
                {
                    ipaddress = "localhost";
                }

                // Register service with consul
                var uri = new Uri(address);
                var registration = new AgentServiceRegistration()
                {
                    ID = $"{consulConfig.ServiceID}-{uri.Host}",
                    Name = consulConfig.ServiceName,
                    Address = $"{uri.Scheme}://{ipaddress}",
                    Port = uri.Port,
                    Tags = new[] { "HSS" }
                };

                logger.LogInformation("Registering with Consul");
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
                consulClient.Agent.ServiceRegister(registration).Wait();

                lifetime.ApplicationStopping.Register(() =>
                {
                    logger.LogInformation("Deregistering from Consul");
                    consulClient.Agent.ServiceDeregister(registration.ID).Wait();
                });
            }
            catch (Exception ex)
            {
                logger.LogError("Failed To Register Service", ex);
            }

            return app;
        }
    }
}
