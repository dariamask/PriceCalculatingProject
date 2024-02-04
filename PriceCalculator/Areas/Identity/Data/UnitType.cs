using PriceCalculatingProject.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace PriceCalculator.Areas.Identity.Data
{
    public class UnitType
    {
        public int ID { get; set; }
        public string? Name { get; set; }   
        public List<Product>? Products { get; set; }
        public List<Category>? Category { get; set; }
        public List<string> UnitTypesList { get; set; } = new List<string>
        {
            "1 килограмм",
            "1 литр",
            "100 грамм",
            "100 миллилитров",
            "1 штука",
        };
    }
}
