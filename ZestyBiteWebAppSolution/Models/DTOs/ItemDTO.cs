namespace ZestyBiteWebAppSolution.Models.DTOs {
    public class ItemDTO {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string ItemCategory { get; set; } = string.Empty;
        public int ItemStatus { get; set; }
        public string ItemDescription { get; set; } = string.Empty;
        public decimal SuggestedPrice { get; set; }
        public string ItemImage { get; set; } = string.Empty;
        public int IsServed { get; set; }
    }
}