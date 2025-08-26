using AutoMapper;
using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using FluentValidation;
using Intersections_EmployeeTrackingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Intersections_EmployeeTrackingSystem.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IReportService _reportService;

        public ReportController(IMapper mapper, IReportService reportService)
        {
            _mapper = mapper;
            _reportService = reportService;
        }

        [HttpGet]
        public IActionResult AddReport()
        {
            var userId = HttpContext.Session.GetInt32("UserID") ?? 0;
            if (userId == 0)
                return Unauthorized();

            var vm = new ReportsViewModel
            {
                UserID = userId
            };

            return PartialView("_ReportAddModal", vm);
        }

        [HttpPost]
        public JsonResult AddReport(ReportsViewModel model, [FromServices] IValidator<ReportsViewModel> validator)
        {
            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(e => new { propertyName = e.PropertyName, errorMessage = e.ErrorMessage })
                    .ToList();

                return Json(new { success = false, errors });
            }
            var entity = _mapper.Map<Report>(model);
            entity.UserID = HttpContext.Session.GetInt32("UserID") ?? 0;

            if (entity == null)
            {
                return Json(new { success = false, message = "Entity değeri null geldi" });
            }

            _reportService.AddReport(entity);
            return Json(new { success = true, message = "Rapor ekleme işlemi başarılı" });

        }
    }
}
