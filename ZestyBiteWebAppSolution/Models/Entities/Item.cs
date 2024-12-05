using System;
using System.Collections.Generic;

namespace ZestyBiteWebAppSolution.Models.Entities;

public partial class Item
{
    public int ItemId { get; set; }

    public string ItemName { get; set; } = null!;

    public string ItemCategory { get; set; } = null!;

    public decimal? OriginalPrice { get; set; }

    public int ItemStatus { get; set; }

    public string ItemDescription { get; set; } = null!;

    public decimal SuggestedPrice { get; set; }

    public string ItemImage { get; set; } = null!;

    public int IsServed { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<SupplyItem> SupplyItems { get; set; } = new List<SupplyItem>();

    public virtual ICollection<TableDetail> TableDetails { get; set; } = new List<TableDetail>();
}
