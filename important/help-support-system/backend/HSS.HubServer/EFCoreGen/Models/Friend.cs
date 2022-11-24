using System;
using System.Collections.Generic;

namespace HSS.HubServer.EFCoreGen.Models;

public partial class Friend
{
    public int Id { get; set; }

    public string UserId { get; set; }

    public string FriendId { get; set; }
}
