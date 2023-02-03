using System.ComponentModel.DataAnnotations.Schema;

namespace EStore.Common.Entities
{
    [Table("Products", Schema = "store")]
    public class ProductEntity :BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public decimal? Price { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string? UpdatedBy { get; set; }
    }
}
