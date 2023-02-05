using EStore.FunctionApp.Extensions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using System;

[assembly: FunctionsStartup(typeof(BookingOfflineApp.Startup))]
namespace BookingOfflineApp
{
    public class Startup : FunctionsStartup
    {
        private static IConfiguration Configuration { set; get; }
        private static IConfigurationRefresher ConfigurationRefresher { set; get; }
        public override void Configure(IFunctionsHostBuilder builder)
        {
            EnsureLoadConfiguration();
			builder.Services.AddServices(Configuration);
        }

        private static void EnsureLoadConfiguration()
        {
            string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddAzureAppConfiguration(options =>
            {
                options.Connect(connectionString)
                    .Select(KeyFilter.Any, LabelFilter.Null)
                    .Select(KeyFilter.Any, "prod")
                        //stop refresher if we use free tier
                        /*.ConfigureRefresh(refreshOptions =>
                             refreshOptions.Register("TestApp:Settings:Message")
                                           .SetCacheExpiration(TimeSpan.FromSeconds(60))
                         )*/;
                ConfigurationRefresher = options.GetRefresher();
            });

            Configuration = configBuilder.Build();
        }

        /// <summary>
        /// Creates a Cosmos DB database and a container with the specified partition key. 
        /// </summary>
        /// <returns></returns>
        //private async Task<CosmosDbService> InitializeCosmosClientInstanceAsync()
        //{
        //    string databaseName = Configuration.GetValue<string>("cosmos_dbname");
        //    string containerName = Configuration.GetValue<string>("cosmos_container");
        //    string account = Configuration.GetValue<string>("cosmos_account");
        //    string key = Configuration.GetValue<string>("cosmos_key");
        //    Microsoft.Azure.Cosmos.CosmosClient client = new Microsoft.Azure.Cosmos.CosmosClient(account, key);
        //    CosmosDbService cosmosDbService = new CosmosDbService(client, databaseName, containerName);
        //    Microsoft.Azure.Cosmos.DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
        //    await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");

        //    return cosmosDbService;
        //}

        //private void GetHttpClient(HttpClient httpClient)
        //{
        //    string eventGridAddress = Configuration.GetValue<string>("event_url");
        //    string eventGridAccessKey = Configuration.GetValue<string>("event_key");
        //    httpClient.BaseAddress = new Uri(eventGridAddress);
        //    httpClient.DefaultRequestHeaders.Accept.Clear();
        //    httpClient.DefaultRequestHeaders.Add("aeg-sas-key", eventGridAccessKey);
        //    httpClient.DefaultRequestHeaders.Accept.Add(
        //        new MediaTypeWithQualityHeaderValue("application/json"));
        //}
    }
}
