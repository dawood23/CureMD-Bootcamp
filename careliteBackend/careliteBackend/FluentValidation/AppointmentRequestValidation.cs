using careliteBackend.DTOs;
using FluentValidation;
using System.Data;

namespace careliteBackend.FluentValidation
{
    public class AppointmentRequestValidation : AbstractValidator<AppointmentRequest>
    {
        public AppointmentRequestValidation()
        {
            RuleFor(x => x.DurationMinutes)
                .NotEmpty()
                .Must(value => new[] { 15, 30, 60 }.Contains(value))
                .WithMessage("Duration must be 15, 30, or 60 minutes.")
                .InclusiveBetween(15, 60).WithMessage("Duration must be between 15 and 60.");

            RuleFor(x => x.StartTime)
                .NotEmpty()
                .Must(BeWithinBusinessHours).WithMessage("Start time must be within business hours (9am–5pm).")
                .Must(NotOnSunday).WithMessage("Appointments cannot be scheduled on Sunday.");

            RuleFor(x => x.PatientID)
                .NotEmpty().WithMessage("Patient is required.").GreaterThan(0);

            RuleFor(x => x.DoctorID)
                .NotEmpty().WithMessage("Doctor is required.").GreaterThan(0);

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(s => new[] { "Scheduled", "Completed", "Cancelled" }.Contains(s))
                .WithMessage("Status must be Scheduled, Completed, or Cancelled.");
        }

        private bool BeWithinBusinessHours(DateTime startTime)
        {
            var start = new TimeSpan(9, 0, 0);
            var end = new TimeSpan(17, 0, 0);
            var appointmentTime = startTime.TimeOfDay;

            return appointmentTime >= start && appointmentTime <= end;
        }

        private bool NotOnSunday(DateTime startTime)
        {
            return startTime.DayOfWeek != DayOfWeek.Sunday;
        }
    }
}
