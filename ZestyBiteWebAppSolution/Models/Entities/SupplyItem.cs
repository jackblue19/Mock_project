using System;
using System.Collections.Generic;

namespace ZestyBiteWebAppSolution.Models.Entities;

public partial class SupplyItem
{
    public int SupplyId { get; set; }

    public int ItemId { get; set; }

    public decimal SupplyItemProfit { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual Supply Supply { get; set; } = null!;
}
