using System;
using System.Collections.Generic;

namespace ZestyBiteWebAppSolution.Models;

public partial class Bill
{
    public int BillId { get; set; }

    public bool BillStatus { get; set; }

    public int PaymentId { get; set; }

    public int AccountId { get; set; }

    public int TableId { get; set; }

    public decimal TotalCost { get; set; }

    public DateTime BillDatetime { get; set; }

    public bool BillType { get; set; }

    public virtual Accounts Account { get; set; } = null!;

    public virtual Payment Payment { get; set; } = null!;

    public virtual ICollection<Profit> Profits { get; set; } = new List<Profit>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual Table Table { get; set; } = null!;
}
