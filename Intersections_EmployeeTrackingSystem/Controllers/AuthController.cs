using AutoMapper;
using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using FluentValidation;
using Intersections_EmployeeTrackingSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Intersections_EmployeeTrackingSystem.Controllers
{
    public class AuthController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public AuthController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Register(RegisterViewModel model, [FromServices] IValidator<RegisterViewModel> validator)
        {
            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(model);
            }


            if (_userService.GetUsers().Any(x => x.UserEmail == model.UserEmail))
            {
                ModelState.AddModelError("UserEmail", "Bu mail zaten kullanımda.");
                return View(model);
            }

            var user = _mapper.Map<User>(model);

            user.Password = _userService.HashPassword(model.Password);

            _userService.AddUser(user);

            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, [FromServices] IValidator<LoginViewModel> validator)
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            HttpContext.Session.Clear();

            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                foreach(var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(model);
            }

            var user = _userService.GetUsers().FirstOrDefault(x => x.UserEmail == model.UserEmail);
            if (user == null || !_userService.VerifyUser(model.UserEmail, model.Password))
            {
                ModelState.AddModelError("", "Yanlış şifre veya mail");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("UserID", user.UserID.ToString()),
                new Claim(ClaimTypes.Email, user.UserEmail),
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            };

            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("MyCookieAuth", principal);

            HttpContext.Session.SetString("UserName", user.UserName);
            HttpContext.Session.SetInt32("UserID", user.UserID);

            return RedirectToAction("Map", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
