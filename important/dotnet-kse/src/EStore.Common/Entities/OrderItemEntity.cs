using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EStore.Common.Entities
{
    [Table("OrderItems", Schema = "store")]
    public class OrderItemEntity : BaseEntity
    {
        [Dapper.Contrib.Extensions.Key]
        [Key]
        public Guid Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int OrderId { get; set; }

        [Dapper.Contrib.Extensions.Computed]
        public OrderEntity Order { get; set; } = null!;
    }
}
