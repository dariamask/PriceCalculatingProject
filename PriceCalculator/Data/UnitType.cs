using System.ComponentModel.DataAnnotations.Schema;

namespace PriceCalculator.Data
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
        //public Dictionary<string, string> UnitTypesList { get; set; } = new Dictionary<string, string>
        //{
        //    {"1 кг.", "1 килограмм" },
        //    {"1 л.", "1 литр" },
        //    {"100 г.", "100 грамм" },
        //    {"100 мл.", "100 миллилитров" },
        //    {"1 шт.", "1 штука" }
        //};
    }
}
