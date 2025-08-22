using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Application.DTOs.Subcategory
{
    public class CreateSubCategoryDto
    {
        [Display(Name = "Alt Kategori Adı"), Required(ErrorMessage = "Alt Kategori Adı boş bırakılamaz!")]
        [StringLength(100,ErrorMessage = "Alt Kategori Adı en fazla 100 karakter olabilir.")]
        public string SubCategoryName { get; set; }
        [Display(Name = "Açıklama")]
        [StringLength(100, ErrorMessage = "Alt Kategori Açıklaması en fazla 100 karakter olabilir.")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Slug boş bırakılamaz!")]
        [StringLength(150, ErrorMessage = "Slug en fazla 150 karakter olabilir.")]
        public string Slug { get; set; } //en-cok-satilan-urunler

        //alt kategori hangi üst kategoriye bağlansın
        [Required(ErrorMessage = "Lütfen bir tane üst kategori seçiniz!")]
        public Guid CategoryId { get; set; }
    }
}
