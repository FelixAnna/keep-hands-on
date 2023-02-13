namespace HSS.SharedModels.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Friends", Schema = "hss")]
    public class FriendEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string FriendId { get; set; } = null!;
    }
}