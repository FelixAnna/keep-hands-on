using System;
using System.Collections.Generic;

namespace HSS.HubServer.EFCoreGen.Models;

public partial class AspNetUserToken
{
    public string UserId { get; set; }

    public string LoginProvider { get; set; }

    public string Name { get; set; }

    public string Value { get; set; }

    public virtual AspNetUser User { get; set; }
}
