namespace ZestyBiteWebAppSolution.Models.DTOs {
    public class PaymentDTO {
        public int PaymentId { get; set; }
        public int PaymentMethod { get; set; }
        public decimal TotalCost { get; set; }
    }
}
