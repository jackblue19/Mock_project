using System;
using System.Collections.Generic;

namespace ZestyBiteWebAppSolution.Models.Entities;

public partial class Bill
{
    public int BillId { get; set; }

    public int BillStatus { get; set; }

    public int PaymentId { get; set; }

    public int TableId { get; set; }

    public decimal TotalCost { get; set; }

    public DateTime BillDatetime { get; set; }

    public int BillType { get; set; }

    public virtual Payment Payment { get; set; } = null!;

    public virtual ICollection<Profit> Profits { get; set; } = new List<Profit>();

    public virtual Table Table { get; set; } = null!;
}
