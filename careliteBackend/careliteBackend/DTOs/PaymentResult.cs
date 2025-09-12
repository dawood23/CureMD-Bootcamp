namespace careliteBackend.DTOs
{
    public class PaymentResult
    {
        public int PaymentID { get; set; }
        public decimal RemainingPending { get; set; }
        public string BillStatus { get; set; } = string.Empty;
    }
}
