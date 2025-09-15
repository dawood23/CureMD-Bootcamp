using careliteBackend.DTOs;
using FluentValidation;

namespace careliteBackend.FluentValidation
{
    public class VisitNoteRequestValidation:AbstractValidator<CreateVisitRequest>
    {
        public VisitNoteRequestValidation() {

            RuleFor(x => x.Content).NotEmpty().WithMessage("Visit Note cannot be empty.");
            RuleFor(x => x.AppointmentID).NotEmpty().GreaterThan(0).WithMessage("AppointmentId cannot be empty and must be greater than zero.");
        }
    }
}
