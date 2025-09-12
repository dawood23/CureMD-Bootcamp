namespace careliteBackend.DTOs
{
    public class PaymentIntentRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "usd";
    }

}
