using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.MVC.Models.ViewModels
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Kategori Adı")]
        public string Name { get; set; }

        [Display(Name = "Kategori İkonu")]
        public string Icon { get; set; }

        [Display(Name = "Ürün Sayısı")]
        public int ProductCount { get; set; }

        [Display(Name = "URL")]
        public string URL { get; set; }

        [Display(Name = "Renk Kodu")]
        public string Color { get; set; }

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Display(Name = "Slug")]
        public string? Slug { get; set; }

        [Display(Name = "Alt Kategori Sayısı")]
        public int SubcategoryCount { get; set; }

        [Display(Name = "Aktif Mi?")]
        public bool IsActive { get; set; }
    }
}
