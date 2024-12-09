namespace ZestyBiteWebAppSolution.Models.DTOs
{
    public class ItemDto
    {
        public int TableId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int ItemQuantity { get; set; }
        public decimal Price { get; set; }
    }

}
