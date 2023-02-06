
using Azure.Messaging.EventGrid;

namespace EStore.SharedServices.Orders.Events
{
    public class OrderDeliverEvent : EventGridEvent
    {
        private const string subject = "Order";
        private const string eventType = "Delivered";
        private const string dataVersion = "1.0";

        public OrderDeliverEvent(int orderId) : base(subject, eventType, dataVersion, new { OrderId = orderId}) 
        {
        }
    }
}
