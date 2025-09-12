using careliteBackend.DTOs;
using careliteBackend.Repository.BillRepository;

namespace careliteBackend.Services.BillService
{
    public class Billservice : IBillService
    {
        private readonly IBillRepository _billRepository;

        public Billservice(IBillRepository billRepository)
        {
            _billRepository = billRepository;
        }

        public async Task<Bill?> GenerateBill(int appointmentID)
        {
            return await _billRepository.GenerateBill(appointmentID);
        }
        public async Task<List<Bill>?> GenerateBillList()
        {
            return await _billRepository.GetBills();
        }

        public async Task<List<PaymentDto>> GetPayments()
        {
            return await _billRepository.GetPayments();
        }
        public async Task<PaymentResult> RecordPayment(PaymentRequest request)
        {
            if (request.Amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.");

            var allowed = new[] { "Cash", "Card" }; 
            if (!allowed.Contains(request.Method, StringComparer.OrdinalIgnoreCase))
                throw new ArgumentException("Unsupported payment method.");

            return await _billRepository.RecordPayment(request);
        }
    }
}
