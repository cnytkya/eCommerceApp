using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Application.DTOs.Product
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        [Display(Name = "Ürün Adı")]
        public string Name { get; set; }
        [Display(Name = "Ürün Açıklaması")]
        public string? Description { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Fiyat negatif olamaz!")]
        [Display(Name = "Ürün Fiyatı")]
        public decimal? Price { get; set; }
        [Display(Name = "Ürün Adedi")]
        public int Stock { get; set; }
        [Display(Name = "Stok Kodu(SKU)")]
        public string? SKU { get; set; }//Stok kodu
        [Display(Name = "Silindi Mi?")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Ürün Resmi")]
        public string? ImageUrl { get; set; }

        //navigation properties için DTO
        [Display(Name = "Alt Kategori Adı")]
        public string SubcategoryName { get; set; }
        [Display(Name = "Kategori Adı")]
        public string CategoryName { get; set; }
        public Guid SubcategoryId { get; set; }
    }
}
