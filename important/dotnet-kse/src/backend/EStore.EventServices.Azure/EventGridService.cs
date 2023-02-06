using Azure.Messaging.EventGrid;
using Azure;
using Microsoft.Extensions.Configuration;

namespace EStore.EventServices.Azure
{
    public class EventGridService
    {
        private readonly EventGridPublisherClient client;

        public EventGridService(IConfiguration confguration)
        {
            client = new EventGridPublisherClient(
                new Uri(confguration["kse:eventhub:endpoint"]!),
                new AzureKeyCredential(confguration["kse:eventhub:access-key"]!));
        }

        public async Task<bool> SendEventAsync(EventGridEvent gridEvent)
        {
            var response = await client.SendEventAsync(gridEvent);
            if (response.IsError)
            {
                Console.WriteLine($"failed to send event： {gridEvent}, reason: {response.ReasonPhrase}");
                return false;
            }

            return true;
        }
    }
}