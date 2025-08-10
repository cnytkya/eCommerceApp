using eCommerceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerceApp.Infrastructure.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            //Opsiyonel: tablo ismini burdan verebiliriz.
            //builder.ToTable("Categories");
            builder.HasKey(x => x.Id);//Primary key
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).HasMaxLength(200);
            builder.Property(x => x.Slug).IsRequired().HasMaxLength(150);

            //Indexes
            builder.HasIndex(x => x.Name).IsUnique();
            builder.HasIndex(x => x.Slug).IsUnique();

            //Query filter for soft delete
            builder.HasQueryFilter(x => !x.IsDeleted);

            //Kategori, artık Product'larla değil, Subcategory'lerle bire-çok ilişki kurar. Ör:Teknoloji/Telefon/telefonlar
            builder.HasMany(x => x.Subcategories)
                .WithOne(c => c.Category)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);//Kategori silinirken, alt kategorilerin silinmesini engelle.
        }
    }
}
/*            Entity Framework Fluent API yaklaşımı
    Entity Framework Fluent API, veritabanı şemasını ve varlıklarınız arasındaki ilişkileri, [Required] gibi özelliklerin aksine, C# kodu kullanarak daha esnek ve merkezi bir şekilde yapılandırmanızı sağlayan bir yaklaşımdır. AppDbContext içindeki OnModelCreating metodunda bir ModelBuilder nesnesi aracılığıyla zincirleme metotlar kullanarak kurallar tanımlarız. Örneğin, Blog adında bir varlığın veritabanındaki tablo adını "Blogs" olarak belirlemek, Id özelliğini birincil anahtar yapmak ve Title özelliğinin zorunlu olmasını sağlamak için şu kodu kullanırız: builder.Entity<Blog>().ToTable("Blogs").HasKey(b => b.Id); builder.Entity<Blog>().Property(b => b.Title).IsRequired().HasMaxLength(200);. Bu yöntem, karmaşık ilişkileri ve kısıtlamaları tek bir yerde topladığı için projenizin daha düzenli ve bakımı kolay olmasını sağlar.
 */