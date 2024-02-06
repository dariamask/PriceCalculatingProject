using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using PriceCalculator.Data;

namespace PriceCalculatingProject.Models
{
    public class CalculatorModel
    {
        //new Category Model
        [StringLength(40) ]
        [Display(Name = "Введите название категории")]
        public string CategoryName { get; set; }
        public string CategoryType { get; set; }
        public bool CategoryIsDelited { get; set; }
        public bool CanUseCalculator { get; set; }

        //select Category Model
        public Dictionary<string, string> CategoryUnitPair { get; set; }
        public List<Category> Categories { get; set; }
        public List<SelectListItem>? SelectListCategories { get; set; } = new List<SelectListItem>{};
        public int SelectedCategory { get; set; }

        //new Item Model

        [Required]
        [Display(Name = "Введите название продукта")]
        [StringLength(50)]
        public string? ProductName { get; set; }
        public bool ProductIsDelited { get; set; }
        public int DelitedProductID { get; set; }

        [Required]
        public decimal ProductPrice { get; set; }

        [Required]
        public decimal ProductQuantity { get; set; }

        public decimal ProductOneUnitPrice { get; set; }
        public List<SelectListItem>? UnitType { get; set; } =
            new List<SelectListItem>
            { 
                new SelectListItem{Value = "kilogramm", Text = "1 килограмм"},
                new SelectListItem{Value = "gramm", Text = "100 грамм"},
                new SelectListItem{Value = "litre", Text = "1 литр"},
                new SelectListItem{Value = "millilitre", Text = "100 миллилитров"},
                new SelectListItem{Value = "unit", Text = "1 штука"},
            };
        public string SelectedUnit {  get; set; }
        public string? ProductNote { get; set; }

        //TableFilling
        public List<Category> ProductsForTable {  get; set; }
        public bool IsBestPriceLightning { get; set; }

        public CalculatorModel() { }
    }
}

