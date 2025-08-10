using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Domain.Entities
{
    public class Category : BaseEntity
    {
        [Display(Name = "Kategori Adı")]
        public string Name { get; set; }
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }
        public string Slug { get; set; }//url dostu keyword'ler. Temel olarak, bir başlığı veya ismi alıp, onu web adresine (URL'e) uygun, küçük harflerden, rakamlardan ve tire işaretlerinden oluşan bir string'e dönüştürür.
        //ÖR: Kategori Adı: Akıllı Telefonlar & Aksesuarlar
        //Slug: akilli-telefon-aksesuarlar
        //ürün Adı: Samsung Galaxy S23 
        //Slug: samsung-galaxy-s23 
        //Muhtemel oluşacak URL: www.ecommerceapp.com/kategori/akilli-telefon

        //Her kategori birden çok alt kategoriye sahip olabilir. bire çok ilişki.
        public ICollection<Subcategory> Subcategories { get; set; } = new List<Subcategory>();
    }
}
