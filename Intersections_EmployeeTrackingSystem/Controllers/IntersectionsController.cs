using AutoMapper;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.DTOs;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using EntityLayer.Enums;
using FluentValidation;
using Intersections_EmployeeTrackingSystem.Models;
using Intersections_EmployeeTrackingSystem.ValidationRules;
using Intersections_EmployeeTrackingSystem.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Sio = System.IO;

namespace Intersections_EmployeeTrackingSystem.Controllers
{
    [Authorize]
    public class IntersectionsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IIntersectionService _intersectionService;
        private readonly IImageService _imageService;
        private readonly IIntersectionChangeRequestService _changeService;

        // DI ile iki bağımlılık birden alıyoruz
        public IntersectionsController(IMapper mapper, 
                                       IIntersectionService intersectionService, 
                                       IImageService imageService,
                                       IIntersectionChangeRequestService changeService)
        {
            _mapper = mapper;
            _intersectionService = intersectionService;
            _imageService = imageService;
            _changeService = changeService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var values = _intersectionService.GetList();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_IntersectionListResponsive", values);
            }

            return View(values);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult DeactiveList()
        {
            var values = _intersectionService.GetDeactives();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeactiveListResponsive", values);
            }

            return View(values);
        }
        [HttpPost]
        public JsonResult AddIntersection(IntersectionViewModel model, [FromServices] IValidator<IntersectionViewModel> validator)
        {
            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(e => new { propertyName = e.PropertyName, errorMessage = e.ErrorMessage })
                    .ToList();

                return Json(new {success = false, errors});
            }

            var intersection = _mapper.Map<Intersection>(model);
            intersection.UserID = HttpContext.Session.GetInt32("UserID") ?? 0;
            intersection.IntersectionStatus = true;
            intersection.UpdatedAt = DateTime.Now;

            if (model.ImageFiles != null && model.ImageFiles.Count > 0)
            {
                foreach (var file in model.ImageFiles)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine("wwwroot/uploads/Images", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        intersection.IntersectionImages.Add(new IntersectionImage
                        {
                            ImagePath = "/uploads/Images/" + fileName
                        });
                    }
                }
            }

            _intersectionService.AddIntersection(intersection);
            return Json(new { success = true });
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var values = _intersectionService.GetWithSubandReport(id);

            if (values == null)
                return NotFound();

            var intersectionVM = _mapper.Map<DetailsVM>(values);

            return View(intersectionVM);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var values = _intersectionService.GetWithSubandReport(id);
            var viewModel = _mapper.Map<IntersectionViewModel>(values);

            return PartialView("_EditModal", viewModel);
        }
        [HttpPost]
        public IActionResult Edit(IntersectionViewModel model, [FromServices] IValidator<IntersectionViewModel> validator)
        {

            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(e => new { propertyName = e.PropertyName, errorMessage = e.ErrorMessage })
                    .ToList();
                return Json(new { success = false, errors});

            }

            var intersection = _intersectionService.GetWithSubandReport(model.IntersectionID);

            if (intersection == null)
                return Json(new { success = false, message = "Kayıt bulunamadı." });

            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role == "User")
            {
                var dto = _mapper.Map<IntersectionUpdateDto>(model);

                if (dto.Substructure == null && model.Subtructure != null)
                    dto.Substructure = _mapper.Map<SubstructureDto>(model.Subtructure);

                if (dto.Locations == null && model.Locations != null)
                    dto.Locations = model.Locations.Select(l => _mapper.Map<LocationDto>(l)).ToList();

                dto.Images ??= new ImageChangeDto();
                if (model.ImageFiles != null && model.ImageFiles.Count > 0)
                {
                    foreach (var file in model.ImageFiles.Where(f => f?.Length > 0))
                    {
                        var rel = SaveStagingHelper.SaveToStaging(file);       
                        dto.Images.AddStagingPaths.Add(rel);
                    }
                }

                dto.Images.DeleteIds = model.DeleteImageIds ?? new List<int>();

                var payload = System.Text.Json.JsonSerializer.Serialize(dto);
                var userId = HttpContext.Session.GetInt32("UserID") ?? 0; 

                _changeService.CreateUpdateRequest(intersection, payload, userId);

                return Json(new { success = true, message = "Değişiklikler admin onayına gönderildi." });
            }


            _mapper.Map(model, intersection);
            intersection.UserID = model.UserID;
            intersection.UpdatedAt = DateTime.Now;

            if (model.ImageFiles != null && model.ImageFiles.Count > 0)
            {
                foreach (var file in model.ImageFiles)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine("wwwroot/uploads/Images", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        intersection.IntersectionImages.Add(new IntersectionImage
                        {
                            ImagePath = "/uploads/Images/" + fileName
                        });
                    }
                }
            }

            _intersectionService.UpdateIntersection(intersection);

            return Json(new { success = true, message = "Güncellendi." });
        }
        [HttpPost]
        public JsonResult SoftDelete(int id)
        {
            var value = _intersectionService.GetByID(id);
            if (value == null)
            {
                return Json(new { success = false, message = "ID bulunamadı" });
            }
            value.IntersectionStatus = false;
            _intersectionService.UpdateIntersection(value);

            return Json(new { success = true, message = "Gizlendi" });
        }
        [HttpPost]
        public JsonResult MakeActive(int id)
        {
            var value = _intersectionService.GetByID(id);

            if(value == null)
            {
                return Json(new { success = false, message = "ID bulunamadı" });
            }

            value.IntersectionStatus = true;
            _intersectionService.UpdateIntersection(value);

            return Json(new { success = true, message = "Aktif edildi" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public JsonResult HardDelete(int id)
        {
            var value = _intersectionService.GetByID(id);
            if (value != null)
            {
                foreach (var img in value.IntersectionImages)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", img.ImagePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                    if (Sio.File.Exists(path))
                        Sio.File.Delete(path);
                }
                _intersectionService.DeleteIntersection(value);
                return Json(new { success = true, message = "Silindi" });
            }
            return Json(new { success = false, message = "ID bulunamadı" });
        }

        public JsonResult GetImages(int id)
        {
            var intersection = _intersectionService.GetByID(id);
            if (intersection != null)
            {
                var images = intersection.IntersectionImages
                         .Select(img => img.ImagePath)
                         .ToList();
                return Json(images);
            }

            return Json(new List<string>());
        }
        [HttpPost]
        public JsonResult DeleteImage(string imagePath)
        {
            try
            {
                var serverPath = Path.Combine("wwwroot", imagePath.TrimStart('/'));
                var role = User.FindFirst(ClaimTypes.Role)?.Value;

                if (role != "Admin")
                {
                    return Json(new { success = false, message = "Kullanıcı resim silemez!" });
                }

                if (Sio.File.Exists(serverPath))
                    Sio.File.Delete(serverPath);

                var imageValue = _imageService.GetList().FirstOrDefault(x => x.ImagePath == imagePath);

                if (imageValue != null)
                {
                    _imageService.DeleteImage(imageValue);
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}
