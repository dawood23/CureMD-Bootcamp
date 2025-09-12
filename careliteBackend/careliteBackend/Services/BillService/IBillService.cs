using careliteBackend.DTOs;
namespace careliteBackend.Services.BillService
{
    public interface IBillService
    {
        Task<Bill?> GenerateBill(int appointmentID);

        Task<List<Bill>?> GenerateBillList();

        Task<List<PaymentDto>> GetPayments();
        Task<PaymentResult> RecordPayment(PaymentRequest request);
    }
}
