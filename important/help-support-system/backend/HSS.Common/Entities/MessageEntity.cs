

using System.ComponentModel.DataAnnotations.Schema;

namespace HSS.Common.Entities;

[Table("hss.Messages")]
public class MessageEntity
{
    public int Id { get; set; }

    public string From { get; set; } = null!;

    public string To { get; set; } = null!;

    public string Content { get; set; } = null!;

    [Column("msg_time")]
    public DateTime? Msg_Time { get; set; }
}
