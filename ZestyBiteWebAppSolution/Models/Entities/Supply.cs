using System;
using System.Collections.Generic;

namespace ZestyBiteWebAppSolution.Models.Entities;

public partial class Supply
{
    public int SupplyId { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal SupplyQuantity { get; set; }

    public decimal SupplyPrice { get; set; }

    public int SupplyStatus { get; set; }

    public DateTime DateImport { get; set; }

    public DateTime DateExpiration { get; set; }

    public int TableId { get; set; }

    public int ItemId { get; set; }

    public string VendorName { get; set; } = null!;

    public string VendorPhone { get; set; } = null!;

    public string VendorAddress { get; set; } = null!;

    public string SupplyCategory { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;

    public virtual ICollection<Profit> Profits { get; set; } = new List<Profit>();

    public virtual Table Table { get; set; } = null!;

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
