namespace ZestyBiteWebAppSolution.Models.DTOs
{
    public class TableDetailDTO {
        public int BillId { get; set; }
        public int TableId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal SuggestedPrice { get; set; }
        public int Quantity { get; set; }
    }

}