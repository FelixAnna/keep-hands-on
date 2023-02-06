using EStore.Common.Models;

namespace EStore.SharedServices.Carts.Contracts
{
    public record GetCartResponse
    {
        public CartModel Cart { get; set; } = null!;
        public IEnumerable<ProductModel> Products { get; set; } = null!;
    }
}
