using careliteBackend.DTOs;

namespace careliteBackend.Repository.BillRepository
{
    public interface IBillRepository
    {
        Task<Bill?> GenerateBill(int appointmentId);

        Task<List<Bill>?> GetBills();

        Task<Bill?> GetBillByID(int id);
        Task<PaymentResult> RecordPayment(PaymentRequest paymentRequest);

        Task<PaymentDto?> GetPaymentByID(int paymentID);

        Task<List<PaymentDto>> GetPayments();
    }

}
