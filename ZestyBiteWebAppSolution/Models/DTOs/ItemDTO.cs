namespace ZestyBiteWebAppSolution.Models.DTOs
{
    public class ItemDTO
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string ItemCategory { get; set; } = string.Empty;
        public int ItemStatus { get; set; }
        public string ItemDescription { get; set; } = string.Empty;
        public decimal SuggestedPrice { get; set; }
        public string ItemImage { get; set; } = string.Empty;
        public int IsServed { get; set; }
        public ICollection<TableDetailDTO> TableDetails { get; set; } = new List<TableDetailDTO>();
    }
    public class EItemDTO
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
    }
}