using EStore.Common.Entities;

namespace EStore.Common.Models
{
    public class OrderModel
    {
        public int OrderId { get; set; }

        public string UserId { get; set; } = null!;

        public OrderStatus Status { get; set; }

        public decimal TotalPrice { get; set; }

        public List<OrderItemModel> Items { get; set; } = null!;

        public decimal GetTotalPrice()
        {
            return Items.Sum(x => x.Quantity * x.UnitPrice ?? 0);
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
