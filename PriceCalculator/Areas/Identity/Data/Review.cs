namespace PriceCalculatingProject.Areas.Identity.Data
{
    public class Review
    {
        public int Id { get; set; }
        public string? CurrentUserId { get; set; }
        public string? UserName { get; set; }
        public string? ReviewText { get; set; }
        public int NumStars { get; set; }
        public bool HasReview { get; set; }
        public bool HasStarred { get; set; }
        public DateTime CreatedDate { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public string? ApplicationUserId { get; set; }
    }
}
