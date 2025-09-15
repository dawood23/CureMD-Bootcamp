using careliteBackend.DTOs;
using FluentValidation;

namespace careliteBackend.FluentValidation
{
    public class UpdateVisitRequestValidator:AbstractValidator<UpdateVisitRequest>
    {
        public UpdateVisitRequestValidator() {
            RuleFor(x => x.content).NotEmpty().WithMessage("The updated content cannot be empty.");
        }
    }
}
