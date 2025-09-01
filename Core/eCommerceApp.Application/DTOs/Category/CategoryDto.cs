using eCommerceApp.Application.DTOs.Subcategory;
using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Application.DTOs.Category
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        [Display(Name = "Kategori Adı")]
        public string Name { get; set; }
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }
        public string Slug { get; set; }
        [Display(Name = "Silindi Mi?")]
        public bool IsDeleted { get; set; }

        //alt kategori SubCategoryDto'ya alalım. Yani alt kategoriler için DTO listesi.
        public ICollection<SubCategoryDto> SubCategories { get; set; }
    }
}
