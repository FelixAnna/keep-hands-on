using System.ComponentModel.DataAnnotations.Schema;

namespace EStore.Common.Entities
{
    [Table("Carts", Schema = "store")]
    public class CartEntity : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string UserId { get; set; } = null!;
    }
}
