using System;
using System.Collections.Generic;

namespace ZestyBiteWebAppSolution.Models.Entities;

public partial class Table
{
    public int TableId { get; set; }
    public int TableCapacity { get; set; }
    public int TableMaintenance { get; set; }
    public int? ReservationId { get; set; }
    public int TableType { get; set; } // Reality or virtual ( 0 -> real, 1 -> virtual )
    public string TableStatus { get; set; } = null!; //Served, Empty, Waiting, Deposit (reservation)
    public string? TableNote { get; set; }
    public int AccountId { get; set; }
    public virtual Account Account { get; set; } = null!;
    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();
    public virtual Reservation? Reservation { get; set; }
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    public virtual ICollection<Supply> Supplies { get; set; } = new List<Supply>();
    public virtual ICollection<TableDetail> TableDetails { get; set; } = new List<TableDetail>();
}
