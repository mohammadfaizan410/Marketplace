using Business.Models;
using Business.Services;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class ProductController : Controller
    {   
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;

        public ProductController(IProductService productService, ICategoryService categoryService, IUserService userService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult AddProduct()
        {
            List<Category> categories = _categoryService.GetAllCategories();
            ViewBag.Categories = categories;
            var model = new ProductModel();
            return View(model);
        }
        [Authorize(Roles = "User")]

        [HttpPost]
        public IActionResult AddProduct(ProductModel model)

        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                }
                List<Category> categories = _categoryService.GetAllCategories();
                ViewBag.Categories = categories;
                return View(model);
            }
            var user = _userService.Query().FirstOrDefault(u => u.UserName == User.Identity.Name);
            model.UserId = user.Id;
            _productService.Add(model);

            return RedirectToAction("Index", "User"); 
        }
        [Authorize(Roles = "User")]

        [HttpGet]
        public IActionResult UpdateProduct(int id)
        {
            Console.WriteLine($"here is the product id {id}");
            var product = _productService.Query().Where(p => p.Id == id).FirstOrDefault();
            List<Category> categories = _categoryService.GetAllCategories();
            ViewBag.Categories = categories;
            if (product != null)
            {
                return View(product);
            }
            else
            {
                return View(product);
            }
        }
        [Authorize(Roles = "User")]

        [HttpPost]
        public IActionResult UpdateProduct(ProductModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if(model.CategoryIdsInput.Count == 0)
            {
                List<Category> categories = _categoryService.GetAllCategories();
                ViewBag.Categories = categories;
                ModelState.AddModelError(nameof(ProductModel.CategoryIdsInput), "At least one category must be selected.");
                return View(model);
            }
            _productService.Update(model);

            return RedirectToAction("UserView", "User");

            
        }


        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            _productService.Delete(id);
            return RedirectToAction("Index", "User");
        }
    }
}
