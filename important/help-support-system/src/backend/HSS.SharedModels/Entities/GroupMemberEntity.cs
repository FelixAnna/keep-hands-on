using System.ComponentModel.DataAnnotations.Schema;

namespace HSS.SharedModels.Entities
{
    [Table("GroupMembers", Schema = "hss")]
    public class GroupMemberEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;

        public int GroupId { get; set; }
        public GroupEntity Group { get; set; } = null!;
    }
}
