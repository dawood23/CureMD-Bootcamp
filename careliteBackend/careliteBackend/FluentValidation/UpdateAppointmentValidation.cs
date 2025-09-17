using careliteBackend.DTOs;
using FluentValidation;

namespace careliteBackend.FluentValidation
{
    public class UpdateAppointmentValidator : AbstractValidator<UpdateAppointmentDto>
    {
        public UpdateAppointmentValidator()
        {
            RuleFor(x => x.AppointmentID)
                .GreaterThan(0).WithMessage("Appointment ID is required.");

            RuleFor(x => x.PatientID)
                .GreaterThan(0).WithMessage("Patient ID is required.");

            RuleFor(x => x.DoctorID)
                .GreaterThan(0).WithMessage("Doctor ID is required.");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Start time is required.");

            RuleFor(x => x.DurationMinutes)
                .Must(value => new[] { 15, 30, 60 }.Contains(value))
                .WithMessage("Duration must be 15, 30, or 60 minutes.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(s => new[] { "Scheduled", "Completed", "Cancelled" }.Contains(s))
                .WithMessage("Status must be Scheduled, Completed, or Cancelled.");

            RuleFor(x => x)
                .Must(dto => dto.Status != "Completed" || DateTime.Now >= dto.StartTime.AddMinutes(dto.DurationMinutes))
                .WithMessage("Cannot mark appointment as Completed before it has finished.");
        }
    }
}
