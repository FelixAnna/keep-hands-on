// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EStore.FunctionApp
{
    public static class OrderPaidFunction
    {
        [FunctionName("Function1")]
        public static async Task Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            log.LogInformation(eventGridEvent.Data.ToString());

            log.LogInformation("C# HTTP trigger function processed a request.");
            log.LogInformation($"Received events: {eventGridEvent}");

        }
    }
}
