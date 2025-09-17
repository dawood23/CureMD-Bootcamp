using careliteBackend.Models;
using FluentValidation;

namespace careliteBackend.FluentValidation
{
    public class PatientValidator : AbstractValidator<Patient>
    {
        public PatientValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters");

            RuleFor(x => x.DOB)
                .LessThan(DateTime.Today).WithMessage("Date of Birth must be in the past")
                .When(x => x.DOB.HasValue);

            RuleFor(x => x.Gender)
                .Must(g => g == "Male" || g == "Female" || string.IsNullOrEmpty(g))
                .WithMessage("Gender must be Male or Female");

            RuleFor(x => x.Phone)
                .Matches(@"^\d{11}$").WithMessage("Phone number must be exactly 11 digits")
                .When(x => !string.IsNullOrEmpty(x.Phone));

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email address")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Address)
                .MaximumLength(500).WithMessage("Address cannot exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.Address));

            RuleFor(x => x.cnic)
                .NotEmpty().WithMessage("CNIC is required")
                .Matches(@"^\d{13}$").WithMessage("CNIC must be exactly 13 digits");
        }
    }
}
