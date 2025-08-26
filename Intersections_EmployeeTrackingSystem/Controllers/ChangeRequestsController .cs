using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using Intersections_EmployeeTrackingSystem.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Intersections_EmployeeTrackingSystem.Controllers
{
    [Authorize]
    public class ChangeRequestsController : Controller
    {

        private readonly IIntersectionChangeRequestService _changeService;
        public ChangeRequestsController(IIntersectionChangeRequestService changeService)
        {
            _changeService = changeService;
        }
        [Authorize(Roles =("Admin"))]
        [HttpGet]
        public IActionResult Pending()
        {
            var list = _changeService.GetPending();
            return View(list); 
        }
        [Authorize(Roles = ("Admin"))]
        [HttpGet]
        public IActionResult DiffDetails(int id)
        {
            var vm = _changeService.GetRequestDetailWithDiff(id);
            return PartialView("DiffDetails", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Approve(int id)
        {
            try
            {
                var adminId = HttpContext.Session.GetInt32("UserID") ?? 0;
                _changeService.Approve(id, adminId);

                if (Request.IsAjaxRequest())
                    return Json(new {success = true, message = "Talep onaylandı ve uygulandı."});

                TempData["ok"] = "Talep onaylandı ve uygulandı.";
            }
            catch (Exception ex)
            {
                if (Request.IsAjaxRequest())
                    return Json(new { success = false, message = ex.Message, id });

                TempData["err"] = ex.Message;
            }
            return RedirectToAction(nameof(Pending));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reject(int id, string note)
        {
            try
            {
                var adminId = HttpContext.Session.GetInt32("UserID") ?? 0;
                _changeService.Reject(id, adminId, note);

                if (Request.IsAjaxRequest())
                    return Json(new { success = true, message = "Talep reddedildi.", id });

                TempData["ok"] = "Talep reddedildi.";
            }
            catch (Exception ex)
            {
                if (Request.IsAjaxRequest())
                    return Json(new { success = false, message = ex.Message, id });

                TempData["err"] = ex.Message;
            }

            return RedirectToAction(nameof(Pending));
        }
    }
}
