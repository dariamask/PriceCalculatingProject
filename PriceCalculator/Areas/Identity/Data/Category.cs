namespace PriceCalculatingProject.Areas.Identity.Data
{
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsDelited { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public string? ApplicationUserID { get; set; }
        public List<Product>? Products { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CategoryUnitType { get; set; }
    }
}