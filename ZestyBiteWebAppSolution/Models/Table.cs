using System;
using System.Collections.Generic;

namespace ZestyBiteWebAppSolution.Models;

public partial class Table
{
    public int TableId { get; set; }

    public int TableCapacity { get; set; }

    public bool TableMaintenance { get; set; }

    public int ReservationId { get; set; }

    public int ItemId { get; set; }

    public bool TableType { get; set; }

    public string TableStatus { get; set; } = null!;

    public string? TableNote { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual Item Item { get; set; } = null!;

    public virtual Reservation Reservation { get; set; } = null!;

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual ICollection<Supply> Supplies { get; set; } = new List<Supply>();

    public virtual ICollection<TableDetail> TableDetails { get; set; } = new List<TableDetail>();
}
