// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using Azure.Messaging.EventGrid;
using EStore.SharedModels.Models;
using EStore.SharedServices.Packages;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace EStore.FunctionApp
{
    public class DeliverOrderFunction
    {
        private readonly IPackService packageService;
        public DeliverOrderFunction(IPackService packageService)
        {
            this.packageService = packageService;
        }

        [FunctionName("DeliverOrder")]
        public async Task Run([EventGridTrigger] EventGridEvent eventGridEvent, ILogger log)
        {
            log.LogInformation(eventGridEvent.Data.ToString());
            var data = JsonConvert.DeserializeObject<OrderModel>(eventGridEvent.Data.ToString());
            await packageService.DeliverOrderAsync(data.OrderId);
            log.LogInformation($"Order status changed to deliverying: {data.OrderId}");
        }
    }
}
