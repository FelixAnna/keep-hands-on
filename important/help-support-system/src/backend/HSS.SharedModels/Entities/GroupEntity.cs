using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HSS.SharedModels.Entities
{
    [Table("Groups", Schema = "hss")]
    public class GroupEntity
    {
        [Key]
        public int GroupId { get; set; }
        public string Name { get; set; } = null!;

        public List<GroupMemberEntity>? Members { get; set; }
    }
}