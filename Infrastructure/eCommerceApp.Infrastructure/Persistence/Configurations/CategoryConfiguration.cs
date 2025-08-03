using eCommerceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerceApp.Infrastructure.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            //Opsiyonel: tablo ismini burdan burdan verebiliriz.
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
