using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PriceCalculatingProject.Areas.Identity.Data;
using PriceCalculatingProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;

namespace PriceCalculatingProject.Controllers
{
    public class HomeController : Controller
    {
        private const string SessionCheckFiltration = "_Lightning";      
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var model = new CalculatorModel();

            model.IsBestPriceLightning = Convert.ToBoolean(HttpContext.Session.GetString(SessionCheckFiltration));

            var userId = GetUserID(HttpContext);

            model.CanUseCalculator = DoesUserHaveAccessToUseCalculator(userId);

            model.Categories = await GetCategoriesAsync(_context, userId, model);

            model.ProductsForTable = CreateListForCalculateTable(model);

            model.SelectListCategories = CreateSelectListItem(model);
            
            return View(model);
        }

        public IActionResult Contacts()
        {
            return View();
        }

        public IActionResult Help()
        {
            return View();
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

        [HttpPost]
        public async Task<IActionResult> AddNewCategory(CalculatorModel model)
        {
            var userId = GetUserID(HttpContext);

            var dbUser = await _context.Users.SingleAsync(K => K.Id == userId);

            var category = new Category
            {
                Name = model.CategoryName,
                IsDelited = false,
                ApplicationUser = dbUser,
                ApplicationUserID = dbUser.Id,
                CreatedDate = DateTime.Now,
                CategoryUnitType = model.UnitType
                                      .Where(p => p.Value == model.SelectedUnit)
                                      .First()
                                      .Text
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddNewProduct(CalculatorModel model)
        {
            var userId = GetUserID(HttpContext);

            model.Categories = await GetCategoriesAsync(_context, userId, model);

            var indexOfSelectedCategory = int.Parse(model.SelectedUnit);

            var newProduct = new Product
            {
                Name = model.ProductName,
                IsDelited = false,
                Price = model.ProductPrice,
                Quantity = model.ProductQuantity,
                Category = model.Categories[indexOfSelectedCategory],
                CategoryID = model.Categories[indexOfSelectedCategory].Id,
                ApplicationUserID = userId,
                ApplicationUser = await _context.Users.SingleAsync(K => K.Id == userId),
                OneUnitPrice = GetProductPrice(model),
                Note = model.ProductNote,
                UnitTypeOld = model.Categories[indexOfSelectedCategory].CategoryUnitType
            };

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public static decimal GetProductPrice(CalculatorModel product)
        {
            decimal OneUnitPrice = 0;
            OneUnitPrice = product.Categories[int.Parse(product.SelectedUnit)].CategoryUnitType switch
            {
                "100 грамм" or "100 миллилитров" => product.ProductPrice * 100 / product.ProductQuantity,
                "1 килограмм" or "1 литр" => product.ProductPrice * 1000 / product.ProductQuantity,
                "1 штука" => product.ProductPrice * 1 / product.ProductQuantity,
                _ => 0
            };

            return OneUnitPrice;
        }

        public IActionResult SwitchModeBestPrice(CalculatorModel model)
        {
            HttpContext.Session.SetString(SessionCheckFiltration, model.IsBestPriceLightning.ToString());

            return RedirectToAction("Index");
        }

        private static string GetUserID(HttpContext httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                return httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            return null;
        }

        private static bool DoesUserHaveAccessToUseCalculator(string userID)
        {
            if (userID == null)
            {
                return false;
            }
            else
            {
                return true;
            }            
        }

        private static async Task<List<Category>> GetCategoriesAsync(ApplicationDbContext context, string userID, CalculatorModel model)
        {
            var categories = await context.Categories
                .Where(x => x.ApplicationUserID == userID && !x.IsDelited)
                .Include(x => x.Products.Where(p => !p.IsDelited))
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            UpdateProductsForBestPrice(categories);

            return categories;
        }

        private static void UpdateProductsForBestPrice(List<Category> categories)
        {
            foreach (var category in categories)
            {
                if (category.Products.Count > 0)
                {
                    decimal lowestPrice = category.Products.Min(bestPrice => bestPrice.OneUnitPrice);

                    foreach (var product in category.Products)
                    {
                        product.IsBestPrice = product.OneUnitPrice == lowestPrice;
                    }
                }
            }
        }

        private static List<Category> CreateListForCalculateTable(CalculatorModel model)
        {
            return model.Categories.Where(x => x.Products.Count > 0).ToList();
        }

        private static List<SelectListItem> CreateSelectListItem(CalculatorModel model)
        {
            List<SelectListItem>? selectList = new List<SelectListItem>();

            for (int i = 0; i < model.Categories.Count; i++)
            {
                var newSelectItem = new SelectListItem
                {
                    Value = i.ToString(),
                    Text = $"{model.Categories[i].Name} - {model.Categories[i].CategoryUnitType}"
                };
                selectList.Add(newSelectItem);
            }

            return selectList;
        }
    }
}