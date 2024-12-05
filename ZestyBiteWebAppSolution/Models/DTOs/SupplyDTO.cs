namespace ZestyBiteWebAppSolution.Models.DTOs
{
    public class SupplyDTO
    {
        public int SupplyId { get; set; }

        public string ProductName { get; set; } = null!;

        public decimal SupplyQuantity { get; set; }

        public decimal SupplyPrice { get; set; }

        public int SupplyStatus { get; set; }

        public DateTime DateImport { get; set; }

        public DateTime DateExpiration { get; set; }

        public int TableId { get; set; }

        public string VendorName { get; set; } = null!;

        public string VendorPhone { get; set; } = null!;

        public string VendorAddress { get; set; } = null!;

        public string SupplyCategory { get; set; }
    }
}
