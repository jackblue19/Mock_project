using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZestyBiteWebAppSolution.Models.DTOs
{
    public class TableDTO
    {
        public int TableId { get; set; }
        public int TableCapacity { get; set; }
        public int TableMaintenance { get; set; }
        public int ReservationId { get; set; }
        public int ItemId { get; set; }
        public int TableType { get; set; }
        public string TableStatus { get; set; } = null!;
        public string? TableNote { get; set; } = null!;
        public int AccountId { get; set; }

    }
}