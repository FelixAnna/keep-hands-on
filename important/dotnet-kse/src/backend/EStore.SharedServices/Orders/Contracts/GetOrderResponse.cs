using EStore.Common.Models;

namespace EStore.SharedServices.Orders.Contracts
{
    public record GetOrderResponse
    {
        public OrderModel Order { get; set; } = null!;
        public IEnumerable<ProductModel> Products { get; set; } = null!;
    }
}
