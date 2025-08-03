namespace eCommerceApp.Domain.Entities
{
    public class Subcategory : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Slug { get; set; }

        //Her bir alt kategori bir üst kategoriye aittir.
        public int CategoryId { get; set; }//CategoryId => Id ilişkisi 
        public Category Category { get; set; }
        //Her bir alt kategori birden çok ürüne(product) sahip olabilir.
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
