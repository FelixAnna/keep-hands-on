namespace EStore.SharedModels.Models
{
    public class CartModel
    {
        public int CartId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string UserId { get; set; } = null!;

        public List<CartItemModel>? Items { get; set; }
    }

    public class CartItemModel
    {
        public Guid Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
