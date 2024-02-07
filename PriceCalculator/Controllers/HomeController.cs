using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PriceCalculatingProject.Models;
using PriceCalculator.Data;
using PriceCalculator.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using PriceCalculator.Services;

namespace PriceCalculatingProject.Controllers
{
    public class HomeController : Controller
    {
        private const string SessionCheckFiltration = "_Lightning";      
        private readonly ApplicationDbContext _context;
        private readonly CalculatorServices _services;
        public HomeController(ApplicationDbContext context, CalculatorServices services)
        {
            _context = context;
            _services = services;
        }
        
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var model = new CalculatorModel();

            if (_context.UnitTypes.ToList().Count < model.UnitType.Count) 
            { 
                foreach(var units in model.UnitType)
                {
                    var newUnit = new UnitType
                    {
                        Name = units.Text
                    };
                    _context.UnitTypes.Add(newUnit);
                    await _context.SaveChangesAsync();
                }
            }        

            model.IsBestPriceLightning = Convert.ToBoolean(HttpContext.Session.GetString(SessionCheckFiltration));

            var userId = _services.GetUserID(HttpContext);

            model.CanUseCalculator = _services.DoesUserHaveAccessToUseCalculator(userId);

            model.Categories = await _services.GetCategoriesAsync(_context, userId, model);

            model.ProductsForTable = _services.CreateListForCalculateTable(model);

            model.SelectListCategories = _services.CreateSelectListItem(model);
            
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
            var userId = _services.GetUserID(HttpContext);

            var dbUser = await _context.Users.SingleAsync(K => K.Id == userId);

            var unitTypeValue = model.UnitType
                                      .Where(p => p.Value == model.SelectedUnit)
                                      .First()
                                      .Text;
           
            var category = new Category
            {
                Name = model.CategoryName,
                IsDelited = false,
                ApplicationUser = dbUser,
                ApplicationUserID = dbUser.Id,
                CreatedDate = DateTime.Now,
                UnitType = _services.GetUnit(_context, model),
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddNewProduct(CalculatorModel model)
        {
            var userId = _services.GetUserID(HttpContext);

            model.Categories = await _services.GetCategoriesAsync(_context, userId, model);

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
                OneUnitPrice = _services.GetProductPrice(model),
                Note = model.ProductNote,
                UnitTypeID = model.Categories[indexOfSelectedCategory].UnitTypeID
            };

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public IActionResult SwitchModeBestPrice(CalculatorModel model)
        {
            HttpContext.Session.SetString(SessionCheckFiltration, model.IsBestPriceLightning.ToString());

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteProduct(int id)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var deletedProduct = await _context.Products.FindAsync(id);

            deletedProduct.IsDelited = true;

            await _context.SaveChangesAsync();          

            return RedirectToAction("Index");
        }
    }
}