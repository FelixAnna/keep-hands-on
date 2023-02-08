using EStore.Common.Models;
using EStore.SharedServices.Packages;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace EStore.OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        private bool EventTypeSubcriptionValidation
            => HttpContext.Request.Headers["aeg-event-type"].FirstOrDefault() ==
               "SubscriptionValidation";

        private readonly IPackService packageService;

        public PackagesController(IPackService packageService)
        {
            this.packageService = packageService;
        }

        [HttpPost("deliver")]
        public async Task<dynamic> UpdateAsync()
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var jsonContent = await reader.ReadToEndAsync();

            // Check the event type.
            // Return the validation code if it's 
            // a subscription validation request. 
            if (EventTypeSubcriptionValidation)
            {
                return await HandleValidation(jsonContent);
            }
            else
            {
                // Handle your custom event
                var events = JArray.Parse(jsonContent);
                foreach (var e in events)
                {
                    // Invoke a method on the clients for 
                    // an event grid notiification.                        
                    var details = JsonConvert.DeserializeObject<GridEvent<OrderModel>>(e.ToString());
                    var response = await packageService.DeliverOrderAsync(details!.Data!.OrderId);
                    Console.WriteLine(response);
                }

                return true;
            }
        }

        [HttpPost("receive")]
        public async Task<dynamic> FinishAsync()
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var jsonContent = await reader.ReadToEndAsync();

            // Check the event type.
            // Return the validation code if it's 
            // a subscription validation request. 
            if (EventTypeSubcriptionValidation)
            {
                return await HandleValidation(jsonContent);
            }
            else
            {
                // Handle your custom event
                var events = JArray.Parse(jsonContent);
                foreach (var e in events)
                {
                    // Invoke a method on the clients for 
                    // an event grid notiification.                        
                    var details = JsonConvert.DeserializeObject<GridEvent<OrderModel>>(e.ToString());
                    var response = await packageService.ReceiveOrderAsync(details!.Data!.OrderId);
                    Console.WriteLine(response);

                }

                return true;
            }
        }

        private static async Task<JsonResult> HandleValidation(string jsonContent)
        {
            var gridEvent =
                JsonConvert.DeserializeObject<List<GridEvent<Dictionary<string, string>>>>(jsonContent)
                    .First();

            // Retrieve the validation code and echo back.
            var validationCode = gridEvent.Data["validationCode"];
            return new JsonResult(new
            {
                validationResponse = validationCode
            });
        }
    }

    public class GridEvent<T> where T : class
    {
        public string Id { get; set; }
        public string EventType { get; set; }
        public string Subject { get; set; }
        public DateTime EventTime { get; set; }
        public T Data { get; set; }
        public string Topic { get; set; }
    }
}
