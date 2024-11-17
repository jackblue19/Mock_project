using System;
using System.Collections.Generic;

namespace ZetstyBite.Models.Entities;

public partial class Profit
{
    public DateTime Date { get; set; }

    public int SupplyId { get; set; }

    public int BillId { get; set; }

    public decimal ProfitAmmount { get; set; }

    public virtual Bill Bill { get; set; } = null!;

    public virtual Supply Supply { get; set; } = null!;
}
