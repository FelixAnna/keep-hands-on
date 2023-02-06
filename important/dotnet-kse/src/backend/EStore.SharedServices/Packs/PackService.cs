using EStore.EventServices.Azure;
using EStore.SharedServices.Orders.Events;
using EStore.SharedServices.Orders.Services;

namespace EStore.SharedServices.Packages
{
    public class PackService : IPackService
    {
        private readonly IOrdersService orderService;
        private readonly EventGridService eventGridService;

        public PackService(IOrdersService orderService, EventGridService eventGridService)
        {
            this.orderService = orderService;
            this.eventGridService = eventGridService;
        }

        public async Task<bool> DeliverOrderAsync(int orderId)
        {
            var result = await orderService.GetByIdAsync(orderId);
            result.Order.Deliver();
            var response = await orderService.UpdateAsync(orderId, result.Order.Status);
            if (response && await eventGridService.SendEventAsync(new OrderDeliverEvent(orderId)))
            {
                return true;
            }

            //failed to send events
            Console.WriteLine($"Publish {nameof(OrderDeliverEvent)} for order: {orderId} failed.");
            return false;
        }

        public async Task<bool> ReceiveOrderAsync(int orderId)
        {
            var result = await orderService.GetByIdAsync(orderId);
            result.Order.Receive();
            var response = await orderService.UpdateAsync(orderId, result.Order.Status);
            if (response && await eventGridService.SendEventAsync(new OrderReceivedEvent(orderId)))
            {
                return true;
            }

            //failed to send events
            Console.WriteLine($"Publish {nameof(OrderReceivedEvent)} for order: {orderId} failed.");
            return false;
        }
    }
}
