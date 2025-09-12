using careliteBackend.DTOs;

namespace careliteBackend.Repository.BillRepository
{
    public interface IBillRepository
    {
        Task<Bill?> GenerateBill(int appointmentId);

        Task<List<Bill>?> GetBills();
        Task<PaymentResult> RecordPayment(PaymentRequest paymentRequest);

        Task<List<PaymentDto>> GetPayments();
    }

}
