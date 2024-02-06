using PriceCalculatingProject.Areas.Identity.Data;

namespace PriceCalculator.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsDelited { get; set; }
        public decimal Price { get; set; }
        public decimal OneUnitPrice { get; set; }
        public bool IsBestPrice { get; set; }
        public decimal? Quantity { get; set; }
        public Category? Category { get; set; }
        public int CategoryID { get; set; }
        public string? Note { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public string? ApplicationUserID { get; set; }
        public UnitType? UnitType { get; set; }
        public int UnitTypeID { get; set; }
    }
}
