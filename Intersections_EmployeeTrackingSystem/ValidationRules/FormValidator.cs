using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using EntityLayer.Enums;
using FluentValidation;
using Intersections_EmployeeTrackingSystem.Models;

namespace Intersections_EmployeeTrackingSystem.ValidationRules
{
    public class FormValidator : AbstractValidator<IntersectionViewModel>
    {
        public FormValidator(IIntersectionService svc)
        {
            RuleFor(x => x.KkcID)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0).WithMessage("Geçerli bir Kkc ID giriniz.")
            .Must((vm, kkc) => vm.IntersectionID == 0
                ? !svc.IsKkcIdInUse(kkc)
                : !svc.IsKkcIdInUse(vm.IntersectionID, kkc))
            .WithMessage("Kkc ID değeri başka bir kavşağa kayıtlı.")
            .OverridePropertyName(nameof(IntersectionViewModel.KkcID));

            RuleFor(x => x.Title).NotEmpty().WithMessage("Kavşak adı boş olamaz.");
            RuleFor(x => x.PedButton).NotNull().WithMessage("Bu alanı boş geçemezsiniz");
            RuleFor(x => x.UPS).NotNull().WithMessage("Bu alanı boş geçemezsiniz");
            RuleFor(x => x.DeviceType)
                .NotEqual(DeviceType.Unknown)
                .WithMessage("Lütfen bir cihaz tipi seçin.")
                .OverridePropertyName(nameof(IntersectionViewModel.DeviceType));

            RuleForEach(x => x.Locations).SetValidator(new LocationViewModelValidator());
        }
    }
    public class LocationViewModelValidator : AbstractValidator<LocationViewModel>
    {
        public LocationViewModelValidator()
        {
            RuleFor(x => x.City)
                .NotEmpty().WithMessage("Şehir adı boş olamaz.");

            RuleFor(x => x.District)
                .NotEmpty().WithMessage("İlçe adı boş olamaz.");

            RuleFor(x => x.Latitude)
                .NotEmpty().WithMessage("Enlem bilgisi boş olamaz.");

            RuleFor(x => x.Longitude)
                .NotEmpty().WithMessage("Boylam bilgisi boş olamaz.");
        }
    }
    public class ReportViewModelValidator : AbstractValidator<ReportsViewModel>
    {
        public ReportViewModelValidator()
        {
            RuleFor(x => x.ReportName).NotEmpty().WithMessage("Rapor adı boş olamaz");
            RuleFor(x => x.ReportDescription).NotEmpty().WithMessage("Açıklama kısmı boş olamaz");
        }
    }

}
