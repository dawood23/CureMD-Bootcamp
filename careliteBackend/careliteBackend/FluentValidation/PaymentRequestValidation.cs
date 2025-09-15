using careliteBackend.DTOs;
using FluentValidation;

namespace careliteBackend.FluentValidation
{
    public class PaymentRequestValidation:AbstractValidator<PaymentRequest>
    {
        public PaymentRequestValidation() { 
        
            RuleFor(x=>x.Amount).NotEmpty().GreaterThan(0);
            RuleFor(x=>x.BillID).NotEmpty().GreaterThan(0);

        }
    }
}
