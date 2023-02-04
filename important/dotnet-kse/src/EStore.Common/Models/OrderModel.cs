using EStore.Common.Entities;
using EStore.Common.Exceptions;

namespace EStore.Common.Models
{
    public class OrderModel
    {
        public OrderModel() {
            Status = OrderStatus.Created;
        }

        public int OrderId { get; set; }

        public string UserId { get; set; } = null!;

        public OrderStatus Status { get; set; }

        public decimal TotalPrice { get; set; }

        public List<OrderItemModel> Items { get; set; } = null!;

        public decimal GetTotalPrice()
        {
            return Items.Sum(x => x.Quantity * x.UnitPrice ?? 0);
        }

        public void Pay()
        {
            PreCheckToChangeStatus(OrderStatus.Created, OrderStatus.Paid);
            //raise domain event
        }

        public void Delivery()
        {
            PreCheckToChangeStatus(OrderStatus.Paid, OrderStatus.Delivering);
            //raise domain event
        }

        public void Receive()
        {
            PreCheckToChangeStatus(OrderStatus.Delivering, OrderStatus.Recived);
            //raise domain event
        }

        public void Finish()
        {
            PreCheckToChangeStatus(OrderStatus.Recived, OrderStatus.Finished);
            //raise domain event
        }

        private void PreCheckToChangeStatus(OrderStatus expectedStatus, OrderStatus targetStatus)
        {
            if (Status != expectedStatus)
            {
                throw new KSEInvalidOperationException($"Failed to change order: {OrderId} to {targetStatus} status, current status: {Status}");
            }

            this.Status = targetStatus;
        }
    }

    public class OrderItemModel
    {
        public Guid Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
