namespace ZestyBiteWebAppSolution.Models.DTOs {
    public class TableDTO {
        public int TableId { get; set; }
        public int TableCapacity { get; set; }
        public int TableMaintenance { get; set; }
        public int TableType { get; set; } // 0 -> real, 1 -> virtual
        public string TableStatus { get; set; } = null!; //Served, Empty, Waiting, Deposit (reservation)
        public string? TableNote { get; set; }
        public int AccountId { get; set; }
    }
}