using System;
using System.Collections.Generic;

namespace ZestyBiteWebAppSolution.Models.Entities;

public partial class Payment
{
    public int PaymentId { get; set; }

    public ulong PaymentMethod { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();
}
