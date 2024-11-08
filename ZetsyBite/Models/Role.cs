using System;
using System.Collections.Generic;

namespace ZetsyBite.Models;

public partial class Role
{
    public sbyte RoleId { get; set; }

    public string RoleDescription { get; set; } = null!;

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
