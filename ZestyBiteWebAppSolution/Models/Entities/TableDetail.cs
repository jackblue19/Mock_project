namespace ZestyBiteWebAppSolution.Models.Entities;

public partial class TableDetail {
    public int? TableId { get; set; }

    public int? ItemId { get; set; }

    public int? Quantity { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual Table Table { get; set; } = null!;
}
