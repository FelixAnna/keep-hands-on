

using System.ComponentModel.DataAnnotations.Schema;

namespace HSS.SharedModels.Entities;

[Table("Messages", Schema = "hss")]
public class MessageEntity
{
    public int Id { get; set; }

    public string From { get; set; } = null!;

    public string To { get; set; } = null!;

    public string Content { get; set; } = null!;

    [Column("msg_time")]
    public DateTime? Msg_Time { get; set; }
}
