using FluentValidation;
using Intersections_EmployeeTrackingSystem.Models;

namespace Intersections_EmployeeTrackingSystem.ValidationRules
{
    public class RegisterValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Kullanıcı adı zorunludur.")
                .MaximumLength(50);

            RuleFor(x => x.UserEmail)
                .NotEmpty().WithMessage("E-posta zorunludur.")
                .EmailAddress().WithMessage("Geçerli bir e-posta girin.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre zorunludur.")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalı.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Şifreler eşleşmiyor.")
                .NotEmpty().WithMessage("Şifre doğrulama zorunludur.");
        }
    }
}
