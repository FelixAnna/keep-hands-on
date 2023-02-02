using EStore.Common.Models;

namespace EStore.SharedServices.Carts.Contracts
{
    public record GetCartResponse
    {
        public CartModel Cart { get; set; }
        public IEnumerable<ProductModel> Products { get; set; } = null!;
    }
}
