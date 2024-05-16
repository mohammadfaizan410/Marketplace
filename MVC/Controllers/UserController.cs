using Business.Models;
using Business.Services;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Diagnostics;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public UserController(ILogger<UserController> logger, IProductService productService, ICategoryService categoryService, IUserService userService, IRoleService roleService)
        {
            _logger = logger;
            _productService = productService;
            _categoryService = categoryService;
            _userService = userService;
            _roleService = roleService;
        }

        [Authorize]
        public IActionResult Index()
        {
            var role = User.FindFirst(ClaimTypes.Role).Value;
            if(role == "Admin")
            {

            var users = _userService.Query().Where(u => u.UserName != User.Identity.Name).ToList();

            if (users.Any())
            {
                ViewBag.Users = users;
            }
            else
            {
                ViewBag.Users = new List<UserModel>();
           }

                ViewBag.Role = User.FindFirst(ClaimTypes.Role).Value;
                ViewBag.AllRoles = _roleService.Query().ToList();
                return View();
            }
            else
            {
                return RedirectToAction("UserView");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult DeleteUser(int id)
        {
            var isSuccess = _userService.DeleteUser(id);

            
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult ToggleAdmin(int id)
        {
            _userService.ToggleAdmin(id);
            return RedirectToAction("Index");

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize(Roles = "User")]
        public IActionResult UserView()
        {
            var user = _userService.Query().FirstOrDefault(u => u.UserName == User.Identity.Name);
            var products = _productService.Query().Where(p => p.UserId == user.Id).ToList();
            ViewBag.products = products;
            ViewBag.Role = User.FindFirst(ClaimTypes.Role).Value;
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult EditAccount()
        {
            UserModel user = _userService.Query().FirstOrDefault(u => u.UserName == User.Identity.Name);
            return View(user);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditAccount(UserModel user)
        {
            if (ModelState.IsValid)
            {
                string res = _userService.Update(user);
                if (res == "notfound")
                {
                    ModelState.AddModelError(nameof(user.RoleId), "User was not found please try again later!");
                    return View(user);
                }
                if(res == "usernameExists")
                {
                    Console.WriteLine("user already exists mann!");
                    ModelState.AddModelError(nameof(user.RoleId), "Username is already taken!");
                    return View(user);
                }
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Index", "User", new { area = "" });

            }

            return View(user);
        
        }

    }
}
