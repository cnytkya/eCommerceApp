using eCommerceApp.Application.DTOs.Subcategory;

namespace eCommerceApp.Application.DTOs.Category
{
    public class CategoryWithProductCountDto
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Slug { get; set; }
        public int ProductCount { get; set; }
        public List<SubCategoryDto> SubCategories { get; set; }
    }
}
