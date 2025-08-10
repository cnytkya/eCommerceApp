using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Domain.Entities
{
    public class Product : BaseEntity
    {
        [Display(Name="Ürün Adı")]
        public string Name { get; set; }
        [Display(Name = "Ürün Açıklaması")]
        public string? Description { get; set; }
        [Range(0,double.MaxValue,ErrorMessage ="Fiyat negatif olamaz!")]
        [Display(Name = "Ürün Fiyatı")]
        public decimal? Price { get; set; }
        [Display(Name = "Ürün Adedi")]
        public int Stock { get; set; }
        [Display(Name = "Stok Kodu(SKU)")]
        public string? SKU { get; set; }//Stok kodu

        //İlişkiler
        public Guid SubcategoryId { get; set; }
        public Subcategory Subcategory { get; set; }//Navigation property
    }
}
