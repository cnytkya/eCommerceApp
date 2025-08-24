using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Domain.Entities
{
    public class Subcategory : BaseEntity
    {
        [Display(Name = "Alt Kategori Adı")]
        public string Name { get; set; }
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }
        public string Slug { get; set; }

        //Her bir alt kategori bir üst kategoriye aittir.
        public Guid CategoryId { get; set; }//CategoryId => Id ilişkisi 
        public Category Category { get; set; }
        //Her bir alt kategori birden çok ürüne(product) sahip olabilir. bire çok ilişki.
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
