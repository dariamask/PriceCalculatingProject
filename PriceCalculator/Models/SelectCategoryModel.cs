using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using PriceCalculatingProject.Areas.Identity.Data;


namespace PriceCalculatingProject.Models
{
    public class SelectCategoryModel
    {
        private readonly ApplicationDbContext? _context;

        [Display(Name = "Введите название категории")]        
        public string? Name { get; set; }
        
        public List<SelectListItem>? Categories { get; set; } =
        new List<SelectListItem>
        {
            new SelectListItem{Value = "NotDefined", Text = "Без категории"},
        };


public string? SelectedCategory { get; set; }

        public async Task CreateNewCategory(ApplicationUser user, SelectCategoryModel newCategory)
        {
            var category = new Category
            {
                Name = newCategory.Name,
                ApplicationUser = user,
            };
            _context.Add(category);
            await _context.SaveChangesAsync();
        }


    }
}
