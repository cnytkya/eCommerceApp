// eCommerceApp.Infrastructure/Seed/AppDbContextSeed.cs

using eCommerceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceApp.Infrastructure.Seed
{
    public static class AppDbContextSeed
    {
        public static void SeedData(ModelBuilder builder)
        {
            // Deterministic GUID'ler için statik değerler
            var electronicsCategoryId = Guid.Parse("C0000000-0000-0000-0000-000000000001");
            var clothingCategoryId = Guid.Parse("C0000000-0000-0000-0000-000000000002");

            var smartphonesSubcategoryId = Guid.Parse("50000000-0000-0000-0000-000000000001");
            var headphonesSubcategoryId = Guid.Parse("60000000-0000-0000-0000-000000000002");

            var smartphoneId = Guid.Parse("70000000-0000-0000-0000-000000000001");
            var headphoneId = Guid.Parse("80000000-0000-0000-0000-000000000002");

            var now = DateTime.UtcNow;

            // Seed Categories
            builder.Entity<Category>().HasData(
                new Category
                {
                    Id = electronicsCategoryId,
                    Name = "Elektronik",
                    Description = "Elektronik cihazlar ve aksesuarlar",
                    Slug = "elektronik",
                    CreatedDate = now,
                    CreatedBy = "System",
                    IsDeleted = false
                },
                new Category
                {
                    Id = clothingCategoryId,
                    Name = "Giyim",
                    Description = "Kadın ve erkek giyim ürünleri",
                    Slug = "giyim",
                    CreatedDate = now,
                    CreatedBy = "System",
                    IsDeleted = false
                }
            );

            // Seed Subcategories
            builder.Entity<Subcategory>().HasData(
                new Subcategory
                {
                    Id = smartphonesSubcategoryId,
                    Name = "Akıllı Telefonlar",
                    Description = "En son teknoloji akıllı telefonlar",
                    Slug = "akilli-telefonlar",
                    CategoryId = electronicsCategoryId, // DÜZELTİLMİŞ KULLANIM
                    CreatedDate = now,
                    CreatedBy = "System",
                    IsDeleted = false
                },
                new Subcategory
                {
                    Id = headphonesSubcategoryId,
                    Name = "Kulaklıklar",
                    Description = "Kablosuz ve gürültü önleyici kulaklıklar",
                    Slug = "kulakliklar",
                    CategoryId = electronicsCategoryId, // DÜZELTİLMİŞ KULLANIM
                    CreatedDate = now,
                    CreatedBy = "System",
                    IsDeleted = false
                }
            );

            // Seed Products
            builder.Entity<Product>().HasData(
                new Product
                {
                    Id = smartphoneId,
                    Name = "Smartphone X",
                    Description = "Latest smartphone with advanced features",
                    Price = 999.99m,
                    Stock = 100,
                    SKU = "SMX-2023",
                    SubcategoryId = smartphonesSubcategoryId, // DÜZELTİLMİŞ KULLANIM
                    CreatedDate = now,
                    CreatedBy = "System",
                    IsDeleted = false
                },
                new Product
                {
                    Id = headphoneId,
                    Name = "Wireless Headphones",
                    Description = "Noise cancelling wireless headphones",
                    Price = 199.99m,
                    Stock = 50,
                    SKU = "WH-2023",
                    SubcategoryId = headphonesSubcategoryId, // DÜZELTİLMİŞ KULLANIM
                    CreatedDate = now,
                    CreatedBy = "System",
                    IsDeleted = false
                }
            );
        }
    }
}
