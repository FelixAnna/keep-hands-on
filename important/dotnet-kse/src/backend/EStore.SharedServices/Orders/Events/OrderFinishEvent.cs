using Azure.Messaging.EventGrid;

namespace EStore.SharedServices.Orders.Events
{
    public class OrderFinishEvent : EventGridEvent
    {
        private const string subject = "Order";
        private const string eventType = "Finished";
        private const string dataVersion = "1.0";

        public OrderFinishEvent(int orderId) : base(subject, eventType, dataVersion, new { OrderId = orderId })
        {
        }
    }
}
