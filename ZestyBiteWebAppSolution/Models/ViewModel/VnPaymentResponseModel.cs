namespace ZestyBiteWebAppSolution.Models.ViewModel
{
    public class VnPaymentResponseModel
    {
        public bool Success { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string OrderDescription { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public string PaymentId { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string VnPayResponseCode { get; set; } = string.Empty;
    }
    public class VnPaymentRequestModel
    {
        public int TableId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal? Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }
}