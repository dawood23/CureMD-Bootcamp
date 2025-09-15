using FluentValidation;
using careliteBackend.DTOs;

namespace careliteBackend.FluentValidation
{
    public class BillValidator : AbstractValidator<Bill>
    {
        public BillValidator()
        {
            RuleFor(x => x.BillID)
                .GreaterThan(0).WithMessage("BillID must be greater than 0.");

            RuleFor(x => x.AppointmentID)
                .GreaterThan(0).WithMessage("AppointmentID is required and must be greater than 0.");

            RuleFor(x => x.PatientID)
                .GreaterThan(0).WithMessage("PatientID is required and must be greater than 0.");

            RuleFor(x => x.PatientName)
                .NotEmpty().WithMessage("Patient name is required.")
                .MaximumLength(200).WithMessage("Patient name must not exceed 200 characters.");

            RuleFor(x => x.DoctorName)
                .NotEmpty().WithMessage("Doctor name is required.")
                .MaximumLength(200).WithMessage("Doctor name must not exceed 200 characters.");

            RuleFor(x => x.GeneratedAt)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Generated date cannot be in the future.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(s => new[] { "Open", "Partial", "Paid" }.Contains(s))
                .WithMessage("Status must be Open, Partial, or Paid.");

            RuleFor(x => x.TotalAmount)
                .GreaterThan(0).WithMessage("Total amount must be greater than zero.");

            RuleFor(x => x.PendingAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Pending amount cannot be negative.")
                .LessThanOrEqualTo(x => x.TotalAmount)
                .WithMessage("Pending amount cannot exceed total amount.");
        }
    }
}
