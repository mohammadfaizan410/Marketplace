using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MVC.Areas.Account.Controllers
{
    [Area("Account")]
    public class HomeController : Controller
    {
        private readonly IUserService _userService;

        public HomeController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "User", new { area = "" });
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserModel user)
        {
            ModelState.Remove(nameof(user.RoleId));
            if (ModelState.IsValid)
            {
                var existingUser = _userService.Query().SingleOrDefault(u => u.UserName == user.UserName && u.Password == u.Password);
                if (existingUser is null)
                {
                    ModelState.AddModelError(nameof(user.Password), "Invalid user name and password!");
                    return View(user);
                }
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, existingUser.UserName),
                    new Claim(ClaimTypes.Role, existingUser.RoleId == 1 ? "Admin" : "User")
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Index", "User", new { area = "" });
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "User", new { area = "" });
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));

                foreach(var error in errors)
                {
                    Console.WriteLine(error);
                }
            }
            if (ModelState.IsValid)
            {
                string res = _userService.Add(model);
                if (res == "exists")
                {
                    ModelState.AddModelError(nameof(UserModel.UserName), "User Already Exists!");
                    return View(model);
                }
                return RedirectToAction("Login");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "User", new { area = "" });
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
