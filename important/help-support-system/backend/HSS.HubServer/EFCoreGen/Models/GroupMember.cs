using System;
using System.Collections.Generic;

namespace HSS.HubServer.EFCoreGen.Models;

public partial class GroupMember
{
    public int Id { get; set; }

    public string UserId { get; set; }

    public int GroupId { get; set; }

    public virtual Group Group { get; set; }
}
