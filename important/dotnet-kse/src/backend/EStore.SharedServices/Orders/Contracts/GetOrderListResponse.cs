using EStore.SharedModels.Models;

namespace EStore.SharedServices.Orders.Contracts
{
    public record GetOrderListResponse
    {
        public int TotalCount { get; set; }
        public IEnumerable<OrderModel> Orders { get; set; } = null!;
        public IEnumerable<ProductModel> Products { get; set; } = null!;
    }
}
