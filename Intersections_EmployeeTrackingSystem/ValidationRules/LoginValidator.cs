using FluentValidation;
using Intersections_EmployeeTrackingSystem.Models;

namespace Intersections_EmployeeTrackingSystem.ValidationRules
{
    public class LoginValidator : AbstractValidator<LoginViewModel>
    {
        public LoginValidator()
        {
            RuleFor(x => x.UserEmail)
                .NotNull().WithMessage("E-posta adresi zorunludur.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

            RuleFor(x => x.Password)
                .NotNull().WithMessage("Şifre zorunludur.")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalı.");
        }
    }
}
