namespace ZestyBiteWebAppSolution.Models.DTOs
{
    public class TableDetailDTO
    {
        public int TableId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public ItemDTO Item { get; set; } // Item name
        
    }
}