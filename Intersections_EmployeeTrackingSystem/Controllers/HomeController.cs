using System.Diagnostics;
using BusinessLayer.Abstract;
using Intersections_EmployeeTrackingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Intersections_EmployeeTrackingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IIntersectionService _intersectionService;

        public HomeController(IIntersectionService intersectionService)
        {
            _intersectionService = intersectionService;
        }
        [Authorize]
        public ActionResult Map()
        {
            return View();
        }
        [Authorize]
        [HttpGet]
        public JsonResult GetAllLocations()
        {
            var intersections = _intersectionService.GetList();


            var locations = intersections
                .Where(x => x.Locations != null && x.Locations.Count > 0)
                .Select(x =>
                {
                    var loc = x.Locations.First();
                    return new
                    {
                        id = x.IntersectionID,
                        kkcid = x.KkcID,
                        title = x.Title,
                        city = loc.City,
                        lat = loc.Latitude,
                        lng = loc.Longitude,
                        deviceType = x.DeviceType.ToString()
                    };
                })
                .ToList();

            return Json(locations);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
