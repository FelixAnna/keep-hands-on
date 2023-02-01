using System.ComponentModel.DataAnnotations.Schema;

namespace EStore.Common.Entities
{
    [Table("CartItems", Schema = "store")]
    public class CartItemEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public string ProductId { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
