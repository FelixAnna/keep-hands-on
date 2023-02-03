using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EStore.Common.Entities
{
    [Table("CartItems", Schema = "store")]
    public class CartItemEntity : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public int CartId { get; set; }

        [Dapper.Contrib.Extensions.Computed]
        public CartEntity Cart { get; set; } = null!;
    }
}
