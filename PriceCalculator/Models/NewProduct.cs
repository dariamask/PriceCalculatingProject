using Microsoft.AspNetCore.Mvc.Rendering;

namespace PriceCalculatingProject.Models
{
    public class NewProduct
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public double Quantity { get; set; }
        public decimal OneUnitPrice { get; set; }
        public ICollection<SelectListItem> TypeOfUnits { get; set; } 
        public string? SelectedUnit { get; set; }

        public NewProduct() 
        {
            TypeOfUnits = new List<SelectListItem>
            {
                new SelectListItem{Value = "kilo", Text = "1 килограмм"},
                new SelectListItem{Value = "gramm", Text = "100 грамм"},
                new SelectListItem{Value = "liter", Text = "1 литр"},
                new SelectListItem{Value = "milliliter", Text = "100 миллилитров"},
                new SelectListItem{Value = "unit", Text = "1 штука"},
            };
        }
    }
}
