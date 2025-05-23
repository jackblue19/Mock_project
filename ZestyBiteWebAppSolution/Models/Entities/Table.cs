﻿using System;
using System.Collections.Generic;

namespace ZestyBiteWebAppSolution.Models.Entities;

public partial class Table
{
    public int TableId { get; set; }

    public int TableCapacity { get; set; }

    public int TableMaintenance { get; set; }

    public int TableType { get; set; }

    public string TableStatus { get; set; } = null!;

    public string? TableNote { get; set; }

    public int AccountId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual ICollection<Supply> Supplies { get; set; } = new List<Supply>();

    public virtual ICollection<TableDetail> TableDetails { get; set; } = new List<TableDetail>();
}
