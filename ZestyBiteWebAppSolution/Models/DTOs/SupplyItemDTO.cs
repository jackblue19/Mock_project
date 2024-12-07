namespace ZestyBiteWebAppSolution.Models.DTOs
{
    public class SupplyItemDTO
    {
        public int SupplyId { get; set; }
        public int ItemId { get; set; }
        public decimal SupplyItemProfit { get; set; }

        // Optionally, include the relevant properties from Item and Supply if needed:
        public string? ProductName { get; set; } // Tên nguồn cung (từ bảng Supply)
        public string? ItemName { get; set; }   // Tên món ăn/sản phẩm (từ bảng Item)
    }
}
