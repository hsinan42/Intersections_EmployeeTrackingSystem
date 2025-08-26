using AutoMapper;
using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Intersections_EmployeeTrackingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Intersections_EmployeeTrackingSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public EmployeeController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult AllEmployees()
        {
            var employees = _userService.GetUsersbyRole("User");

            var employeeVMs = _mapper.Map<List<EmployeeViewModel>>(employees);


            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EmployeeListResponsive", employeeVMs);
            }

            return View(employeeVMs);
        }

        [HttpGet]
        public IActionResult GetEmployee(int id)
        {
            var employee = _userService.GetUserWithIntersections(id);
            if (employee == null) return NotFound();

            var employeeVM = _mapper.Map<EmployeeViewModel>(employee);

            return View(employeeVM);
        }
        [HttpPost]
        public IActionResult DeleteEmployee(int id)
        {
            var employee = _userService.GetByID(id);
            if (employee == null)
            {
                return Json(new { success = false, message = "Kullanıcı bulunamadı" });
            }

            _userService.DeleteUser(employee);
            return Json(new { success = true, message = "Kullanıcı Silindi" });

        }
    }
}
