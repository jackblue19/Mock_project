using System;
using System.Collections.Generic;

namespace ZetsyBite.Models;

public partial class Reservation
{
    public int ReservationId { get; set; }

    public int BillId { get; set; }

    public int TableId { get; set; }

    public DateTime ReservationStart { get; set; }

    public DateTime ReservationEnd { get; set; }

    public virtual Bill Bill { get; set; } = null!;

    public virtual Table Table { get; set; } = null!;

    public virtual ICollection<Table> Tables { get; set; } = new List<Table>();
}
