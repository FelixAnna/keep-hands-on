using EStore.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.SharedServices.Products.Contracts
{
    public record GetProductResponse
    {
        public int TotalCount { get; set; }
        public IEnumerable<ProductModel> Products { get; set; } = null!;
    }
}
