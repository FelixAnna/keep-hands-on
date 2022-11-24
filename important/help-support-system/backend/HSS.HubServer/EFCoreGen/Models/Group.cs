using System;
using System.Collections.Generic;

namespace HSS.HubServer.EFCoreGen.Models;

public partial class Group
{
    public int GroupId { get; set; }

    public string Name { get; set; }

    public virtual ICollection<GroupMember> GroupMembers { get; } = new List<GroupMember>();
}
