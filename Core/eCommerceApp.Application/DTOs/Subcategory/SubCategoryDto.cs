using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Application.DTOs.Subcategory
{
    public class SubCategoryDto
    {
        public Guid Id { get; set; }
        [Display(Name = "Alt Kategori Adı")]
        public string Name { get; set; }
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }
        public string Slug { get; set; }
        [Display(Name = "Silindi Mi?")]
        public bool IsDeleted { get; set; }

        //alt kategorinin hangi üst kategoriye ait olduğunu göstermek için
        public Guid CategoryId { get; set; }
    }
}
