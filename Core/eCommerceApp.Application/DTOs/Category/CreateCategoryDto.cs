using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Application.DTOs.Category
{
    public class CreateCategoryDto
    {
        [Display(Name = "Kategori Adı"), Required(ErrorMessage = "Kategori Adı boş bırakılamaz!")]
        public string Name { get; set; }
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Slug boş bırakılamaz!")]
        public string Slug { get; set; } //en-cok-satilan-urunler
    }
}
