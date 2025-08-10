using eCommerceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerceApp.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            //tabloya isim vermek istersem.
            //builder.ToTable("Products"); zaten appdbcontext'e Products olarak tanımlanmıştır.
            builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Description).IsRequired(false).HasMaxLength(1000);//IsRequired(false) =>nullable(string) olduğu için zorunlu mu(bool) kısmına hayır diyoruz.
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");//toplam basamak sayısı 18, virgülden sonraki basamak sayısı 2 olsun.
            builder.Property(p => p.Stock).IsRequired().HasDefaultValue(0);
            builder.Property(p => p.SKU).IsRequired(false).HasMaxLength(50);

            //İlişkiler: Ürün üst kategoriye değil alt kategoriye ait olsun.
            builder.HasOne(p => p.Subcategory).WithMany(p=>p.Products).HasForeignKey(p=>p.SubcategoryId).OnDelete(DeleteBehavior.Restrict);//Alt kategori silinirken ürünlerin silinmesini engelle.

            //Indexes
            builder.HasIndex(x => x.Name).IsUnique();
            builder.HasIndex(x => x.SKU).IsUnique();
            builder.HasIndex(x => x.SubcategoryId);

            //Query filter for soft delete
            builder.HasQueryFilter(x => !x.IsDeleted);

        }
    }
}
