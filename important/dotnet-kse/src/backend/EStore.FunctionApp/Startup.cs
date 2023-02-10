using EStore.FunctionApp.Extensions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

[assembly: FunctionsStartup(typeof(EStore.FunctionApp.Startup))]
namespace EStore.FunctionApp
{
    public class Startup : FunctionsStartup
    {
        private static IConfiguration Configuration { set; get; }
        //private static IConfigurationRefresher ConfigurationRefresher { set; get; }
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //EnsureLoadConfiguration();
            builder.Services.AddServices(Configuration);
        }
    }
}
