using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EStore.Common.Entities
{
    [Table("Orders", Schema = "store")]
    public class OrderEntity : BaseEntity
    {
        [Dapper.Contrib.Extensions.Key]
        [Key]
        public int OrderId { get; set; }

        public string UserId { get; set; } = null!;

        public OrderStatus Status { get; set; }

        public decimal? TotalPrice { get; set; }

        [Dapper.Contrib.Extensions.Computed]
        public List<OrderItemEntity> Items { get; set; } = null!;
    }

    public enum OrderStatus
    {
        Created,
        Paid,
        Delivering,
        Recived,
        Finished
    }
}
