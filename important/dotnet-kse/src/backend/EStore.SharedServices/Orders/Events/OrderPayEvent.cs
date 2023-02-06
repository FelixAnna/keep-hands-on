
using Azure.Messaging.EventGrid;

namespace EStore.SharedServices.Orders.Events
{
    public class OrderPayEvent : EventGridEvent
    {
        private const string subject = "Order";
        private const string eventType = "Paid";
        private const string dataVersion = "1.0";

        public OrderPayEvent(int orderId) : base(subject, eventType, dataVersion, new { OrderId = orderId}) 
        {
        }
    }
}
