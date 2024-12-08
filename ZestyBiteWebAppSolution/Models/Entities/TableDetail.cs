using System;
using System.Collections.Generic;

namespace ZestyBiteWebAppSolution.Models.Entities;

public partial class TableDetail {
    public int TableId { get; set; }

    public int ItemId { get; set; }

    public int BillId { get; set; }

    public decimal OriPrice { get; set; }

    public decimal SugPrice { get; set; }

    public int Quantity { get; set; }

    public virtual Bill Bill { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;

    public virtual Table Table { get; set; } = null!;
}