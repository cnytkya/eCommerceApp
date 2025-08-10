using eCommerceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerceApp.Infrastructure.Persistence.Configurations
{
    public class SubcategoryConfiguration : IEntityTypeConfiguration<Subcategory>
    {
        public void Configure(EntityTypeBuilder<Subcategory> builder)
        {
            //primary key
            builder.HasKey(s=>s.Id);
            //Özellikler için kurallar
            builder.Property(s=>s.Name).IsRequired().HasMaxLength(100);
            builder.Property(s=>s.Description).IsRequired(false).HasMaxLength(100);
            builder.Property(s=>s.Slug).IsRequired().HasMaxLength(150);

            //ilişkiler: Bir alt kategori bir üst kategoriye bağlıdır.
            builder.HasOne(s => s.Category).WithMany(s => s.Subcategories).HasForeignKey(s=>s.CategoryId).OnDelete(DeleteBehavior.Restrict);//Alt kategori silinirken ürünleirn silinmesini engelle.

            //bir alt kategorinin birden fazla ürünü olabilir.
            builder.HasMany(s=>s.Products).WithOne(s=>s.Subcategory).HasForeignKey(s=>s.SubcategoryId).OnDelete(DeleteBehavior.Restrict);//Alt kategori silinirken ürünleirn silinmesini engelle.

            //Indexes
            builder.HasIndex(x => x.Name).IsUnique();
            builder.HasIndex(x => x.Slug).IsUnique();

            //Query filter for soft delete
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}