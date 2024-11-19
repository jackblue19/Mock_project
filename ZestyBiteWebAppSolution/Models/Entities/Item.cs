using System;
using System.Collections.Generic;

namespace ZestyBiteWebAppSolution.Models.Entities;

public partial class Item
{
    public int ItemId { get; set; }

    public string ItemName { get; set; } = null!;

    public string ItemCategory { get; set; } = null!;

    public int ItemStatus { get; set; }

    public string ItemDescription { get; set; } = null!;

    public decimal SuggestedPrice { get; set; }

    public string ItemImage { get; set; } = null!;

    public int IsServed { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Supply> Supplies { get; set; } = new List<Supply>();

    public virtual ICollection<TableDetail> TableDetails { get; set; } = new List<TableDetail>();

    public virtual ICollection<Table> Tables { get; set; } = new List<Table>();

    public virtual ICollection<Supply> SuppliesNavigation { get; set; } = new List<Supply>();
}
