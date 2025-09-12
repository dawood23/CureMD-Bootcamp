using careliteBackend.DTOs;
using FluentValidation;
using System.Data;

namespace careliteBackend.FluentValidation
{
    public class AppointmentRequestValidation:AbstractValidator<AppointmentRequest>
    {
        public AppointmentRequestValidation() { 
            RuleFor(x=>x.DurationMinutes).NotEmpty()
                .Must(value => new[] {15,30,60}.Contains(value)).InclusiveBetween(15,60).WithMessage("Duration Must be between 15 and 60");

            RuleFor(x => x.StartTime).NotEmpty().Must(BeWithinBusinessHours).WithMessage("Start Time must be within business hours (9am-5pm)");

            RuleFor(x => x.PatientID).NotEmpty();

            RuleFor(x=>x.DoctorID).NotEmpty();
        }

        private bool BeWithinBusinessHours(DateTime startTime)
        {
            var start = new TimeSpan(9, 0, 0);
            var end = new TimeSpan(17, 0, 0);

            var appointmentTime = startTime.TimeOfDay;

            return appointmentTime>=start && appointmentTime<=end;
        }
    }
}
