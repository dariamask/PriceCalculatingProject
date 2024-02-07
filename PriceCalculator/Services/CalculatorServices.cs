using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PriceCalculatingProject.Models;
using PriceCalculator.Data;
using System.Security.Claims;

namespace PriceCalculator.Services
{
    public class CalculatorServices
    {
        public decimal GetProductPrice(CalculatorModel product)
        {
            var OneUnitPrice = product.Categories[int.Parse(product.SelectedUnit)].UnitTypeID switch
            {
                2 or 4 => product.ProductPrice * 100 / product.ProductQuantity,
                1 or 3 => product.ProductPrice * 1000 / product.ProductQuantity,
                5 => product.ProductPrice * 1 / product.ProductQuantity,
                _ => 0
            };

            return OneUnitPrice;
        }

        public string GetUserID(HttpContext httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                return httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            return null;
        }

        internal bool DoesUserHaveAccessToUseCalculator(string userID)
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

        internal async Task<List<Category>> GetCategoriesAsync(ApplicationDbContext context, string userID, CalculatorModel model)
        {
            var categories = await context.Categories
                .Where(x => x.ApplicationUserID == userID && !x.IsDelited)
                .Include(x => x.Products.Where(p => !p.IsDelited))
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            UpdateProductsForBestPrice(categories);

            return categories;
        }

        internal void UpdateProductsForBestPrice(List<Category> categories)
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

        internal List<Category> CreateListForCalculateTable(CalculatorModel model)
        {
            return model.Categories.Where(x => x.Products.Count > 0).ToList();
        }

        internal List<SelectListItem> CreateSelectListItem(CalculatorModel model)
        {
            List<SelectListItem>? selectList = new List<SelectListItem>();

            for (int i = 0; i < model.Categories.Count; i++)
            {
                var newSelectItem = new SelectListItem
                {
                    Value = i.ToString(),
                    Text = $"{model.Categories[i].Name} - {model.Categories[i].UnitType.Name}"
                };
                selectList.Add(newSelectItem);
            }

            return selectList;
        }

        internal UnitType GetUnit(ApplicationDbContext dbContext, CalculatorModel model)
        {
            var unitText = model.UnitType
                          .Where(p => p.Value == model.SelectedUnit)
                          .First()
                          .Text;
            var unit = dbContext.UnitTypes.Single(u => u.Name == unitText);


            return unit;
        }
    }
}
