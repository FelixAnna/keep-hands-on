using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace ProductApi
{
    public class EndpointHealthCheck : IHealthCheck
    {
        private readonly ServiceSettings settings;
        public EndpointHealthCheck(IOptions<ServiceSettings> settings)
        {
            this.settings = settings.Value;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            Ping ping = new();

            var reply = await ping.SendPingAsync(settings.Host);
            if(reply.Status != IPStatus.Success)
            {
                return HealthCheckResult.Unhealthy();
            }

            return HealthCheckResult.Healthy();
        }
    }
}
