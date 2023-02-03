using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EStore.Common.Entities
{
    [Table("Carts", Schema = "store")]
    public class CartEntity : BaseEntity
    {
        [Key]
        public int CartId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string UserId { get; set; } = null!;

        [Dapper.Contrib.Extensions.Computed]
        public List<CartItemEntity> Items { get; set; } = null!;
    }
}
