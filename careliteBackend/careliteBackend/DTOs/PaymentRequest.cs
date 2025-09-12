namespace careliteBackend.DTOs
{
    public class PaymentRequest
    {
        public int BillID { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;
        public int RecordedBy { get; set; }
    }
}
