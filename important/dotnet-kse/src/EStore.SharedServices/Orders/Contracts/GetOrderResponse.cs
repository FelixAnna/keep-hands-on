using EStore.Common.Models;

namespace EStore.SharedServices.Orders.Contracts
{
    public record GetOrderResponse
    {
        public int TotalCount { get; set; }
        public IEnumerable<OrderModel> Orders { get; set; } = null!;
        public IEnumerable<ProductModel> Products { get; set; } = null!;
    }
}
