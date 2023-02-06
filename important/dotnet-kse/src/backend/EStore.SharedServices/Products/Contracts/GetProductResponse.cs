using EStore.Common.Models;

namespace EStore.SharedServices.Products.Contracts
{
    public record GetProductResponse
    {
        public int TotalCount { get; set; }
        public IEnumerable<ProductModel> Products { get; set; } = null!;
    }
}
