using FluentValidation;
using careliteBackend.DTOs;
using System.Text.RegularExpressions;
using careliteBackend.Models;

namespace careliteBackend.FluentValidation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.UserID)
                .GreaterThan(0).WithMessage("UserID must be greater than 0.");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(100).WithMessage("Username cannot exceed 100 characters.")
                .Must(NotContainHtml).WithMessage("Username cannot contain HTML or script tags.");

            RuleFor(x => x.PasswordHash)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("Email must be valid.")
                .Must(NotContainHtml).WithMessage("Email cannot contain HTML or script tags.");


            RuleFor(x => x.RoleID)
                .GreaterThan(0).WithMessage("RoleID must be greater than 0.");
        }


        private bool NotContainHtml(string? input)
        {
            if (string.IsNullOrEmpty(input))
                return true;

            return !Regex.IsMatch(input, "<.*?>");
        }
    }
}
