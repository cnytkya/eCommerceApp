using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Application.DTOs.Subcategory
{
    public class EditSubCategoryDto
    {
        public Guid Id { get; set; }//get isteğine karşı Id bilgisini de getirmek için.
        [Display(Name = "Alt Kategori Adı")]
        public string CategoryName { get; set; }
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }
        [StringLength(150, ErrorMessage = "Slug en fazla 150 karakter olabilir.")]
        public string Slug { get; set; }

        [Required(ErrorMessage = "Lütfen bir tane üst kategori seçiniz!")]
        public Guid CategoryId { get; set; }
    }
}
