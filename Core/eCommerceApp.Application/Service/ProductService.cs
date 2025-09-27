using AutoMapper;
using eCommerceApp.Application.DTOs.Product;
using eCommerceApp.Application.Interface.Repositories.Categories;
using eCommerceApp.Application.Interface.Repositories.Products;
using eCommerceApp.Application.Interface.Services;
using eCommerceApp.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace eCommerceApp.Application.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepositories _productRepo;
        private readonly ISubCategoryRepo _subCategoryRepo;
        private readonly IMapper _mapper;

        public ProductService(IProductRepositories productRepo, ISubCategoryRepo subCategoryRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _subCategoryRepo = subCategoryRepo;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            //Ürünleri alt kategoriyle birlikte listele.
            var products = await _productRepo.GetAllProductsWithSubcategoryAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
        public async Task<ProductDto?> GetProductByIdAsync(Guid id)
        {
            var product = await _productRepo.GetProductWithSubcategoryIdAsync(id);
            return _mapper.Map<ProductDto?>(product);
        }

        public async Task<(bool succedeed, IEnumerable<string> errors)> CreateProductAsync(CreateProductDto createProductDto,IFormFile formFile)
        {
            //Aynı SKU'ya sahip başka bir ürün var mı
            var existingProduct =  await _productRepo.GetFirstOrDefaultAsync(p => p.SKU == createProductDto.SKU);
            if (existingProduct != null)
            {
                return (false, new[] { "Bu stok koduna(SKU) sahip bir ürün zaten var" });
            }

            var product = _mapper.Map<Product>(createProductDto);
            await _productRepo.AddAsync(product);
            await _productRepo.SaveChangesAync();
            return (true, Enumerable.Empty<string>());
        }
        public async Task<(bool succedeed, IEnumerable<string> errors)> UpdateProductAsync(EditProductDto editProductDto)
        {
            var product = await _productRepo.GetByIdAsync(editProductDto.Id);
            if (product == null)
            {
                return (false, new[] { "Güncellenecek ürün bulunamadı" });
            }
            //SKU değişmisşe aynı SKU'ya sahip başka ürün var mı kontrolü
            if(product.SKU != editProductDto.SKU)
            {
                var exsitingProduct = await _productRepo.GetFirstOrDefaultAsync(p => p.SKU == editProductDto.SKU);
                if(exsitingProduct != null)
                {
                    return (false, new[] { "Bu stok koduna (SKU) sahip bir ürün zaten var." });
                }
            }
            //DTO'dan entity'ye mapping
            _mapper.Map(editProductDto, product);
            _productRepo.Update(product);
            await _productRepo.SaveChangesAync();
            return (true, Enumerable.Empty<string>());
        }

        public async Task<(bool succedeed, IEnumerable<string> errors)> DeleteProductByIdAsync(Guid id)
        {
            //Önce silinecek ürünü veritabanında getir.
            var product = await _productRepo.GetByIdAsync(id);
            if(product == null)
            {
                return (false, new[] { "Silinecek ürün bulunamadı." });
            }
            _productRepo.Remove(product);
            await _productRepo.SaveChangesAync();
            return (true, Enumerable.Empty<string>());
        }
    }
}
