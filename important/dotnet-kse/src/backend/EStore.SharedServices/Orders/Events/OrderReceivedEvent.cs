using Azure.Messaging.EventGrid;

namespace EStore.SharedServices.Orders.Events
{
    public class OrderReceivedEvent : EventGridEvent
    {
        private const string subject = "Order";
        private const string eventType = "Received";
        private const string dataVersion = "1.0";

        public OrderReceivedEvent(int orderId) : base(subject, eventType, dataVersion, new { OrderId = orderId })
        {
        }
    }
}
