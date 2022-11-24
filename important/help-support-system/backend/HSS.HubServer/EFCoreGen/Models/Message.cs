using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HSS.HubServer.EFCoreGen.Models;

public partial class Message
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    public string From { get; set; }

    public string To { get; set; }

    public string Content { get; set; }

    public DateTime? MsgTime { get; set; }
}
