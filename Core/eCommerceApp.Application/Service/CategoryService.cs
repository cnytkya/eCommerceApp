using AutoMapper;
using eCommerceApp.Application.DTOs.Category;
using eCommerceApp.Application.DTOs.Subcategory;
using eCommerceApp.Application.Interface.Repositories.Categories;
using eCommerceApp.Application.Interface.Repositories.Products;
using eCommerceApp.Application.Interface.Services;
using eCommerceApp.Domain.Entities;

namespace eCommerceApp.Application.Service
{
    public class CategoryService : ICategoryService
    {
        //DI
        private readonly ICategoryRepo _categoryRepo;
        private readonly ISubCategoryRepo _subCategoryRepo;
        private readonly IProductRepositories _productRepositories;
        private IMapper _mapper;

        public CategoryService(ICategoryRepo categoryRepo, ISubCategoryRepo subCategoryRepo, IProductRepositories productRepositories, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _subCategoryRepo = subCategoryRepo;
            _productRepositories = productRepositories;
            _mapper = mapper;
        }

        //kategori CRUD operasyonları
        public async Task<(bool succeeded, IEnumerable<string> errors)> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            //Aynı slug'a sahip başka bir kategori var mı?
            var existingCategory = await _categoryRepo.GetFirstOrDefaultAsync(c => c.Slug == createCategoryDto.Slug);
            if (existingCategory != null)
            {
                return (false, new[] { "Bu slug'a sahip bir kategori zaten mevcut." });
            }
            var category = _mapper.Map<Category>(createCategoryDto);

            await _categoryRepo.AddAsync(category);//yeni nesne oluşturuldu.
            await _categoryRepo.SaveChangesAync(); //yeni oluşturulan nesneyi veritabanına yansıt.

            return (true, Enumerable.Empty<string>());
        }

        public async Task<(bool succeeded, IEnumerable<string> errors)> CreateSubCategoryAsync(CreateSubCategoryDto createSubCategoryDto)
        {
            //Aynı slug'a sahip başka bir alt kategori var mı?
            var existingSubCategory = await _subCategoryRepo.GetFirstOrDefaultAsync(c => c.Slug == createSubCategoryDto.Slug);
            if (existingSubCategory != null)
            {
                return (false, new[] { "Bu slug'a sahip bir alt kategori zaten mevcut." });
            }
            var subCategory = _mapper.Map<Subcategory>(createSubCategoryDto);

            await _subCategoryRepo.AddAsync(subCategory);//yeni nesne oluşturuldu.
            await _subCategoryRepo.SaveChangesAync(); //yeni oluşturulan nesneyi veritabanına yansıt.

            return (true, Enumerable.Empty<string>());
        }

        public async Task<(bool succeeded, IEnumerable<string> errors)> DeleteCategoryAsync(Guid id)
        {
            //veritabanından kaydı alalım yani kaydı id'sine göre çekelim.
            var category = await _categoryRepo.GetByIdAsync(id);
            //aldığımız kayıt veritabanında var mı(null kontrolü). eğer null ise
            if (category == null)
            {
                return (false, new[] { "Silinecek kategori bulunamadı" });
            }
            //bu kategoriye ait alt kategoriler var mı?
            var subCategories = await _subCategoryRepo.FindAsync(c => c.CategoryId == id);
            if (subCategories.Any())
            {
                return (false, new[] { "Bu kategoriye bağlı alt kategoriler mevcut. Lütfen önce bağlı alt kategoriyi(kategorileri) silin." });
            }
            //herhangi bir sorun yoksa o zaman nesneyi veritabanından sil.
            _categoryRepo.Remove(category);
            await _subCategoryRepo.SaveChangesAync();//nesne silindikten sonra veritabanına değişikliği uygula.
            return (true, Enumerable.Empty<string>());
        }

        public async Task<(bool succeeded, IEnumerable<string> errors)> DeleteSubCategoryAsync(Guid id)
        {
            //önce silinecek alt kategoriyi id'ye göre çağır.
            var subCategory = await _subCategoryRepo.GetByIdAsync(id);
            //category boş mu kontrolü
            if (subCategory == null)
            {
                return (false, new[] { "Silinecek alt kategori bulunamadı!" });
            }
            //eğer silinecek alt kategoriye bağlı ürünler var ise silinmesini engelle. burda veritabanındaki ürünlerin alt kategorilerle ilişkisini sorgulamamız lazım. Eğer ilişkili alt kategoriye ait ürün veya ürünler var ise kategorinin silinmesini engelle.
            //_productRepositories üzerinden bağlı ürün var mı kontrolü.

            var productsInSubcategory = await _productRepositories.GetFirstOrDefaultAsync(p=>p.SubcategoryId == id);//bağlı ürün varsa productsInSubcategory değişkenine atanır.

            //productsInSubcategory değişkeninde eğer ürün varsa silinmesini engelle
            if (productsInSubcategory != null)
            {
                return (false, new[] { "Bu alt kategoriye bvağlı ürünler mevcut. Lütfen ürünleri siliniz ya da başka bir yere taşıyınız." });
            }
            //eğer silinecek kategoride ürün yoksa o zaman silmeyi gerçekleştir.
            _subCategoryRepo.Remove(subCategory);
            await _subCategoryRepo.SaveChangesAync();
            return (true, Enumerable.Empty<string>());
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            //veritabanından kategorileri çek.
            var categories = await _categoryRepo.GetAllCategoriesWithSubcategoriesAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(Guid id)
        {
            //kategoriyi id'ye göre getir.
            var category = await _categoryRepo.GetByIdAsync(id);
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<IEnumerable<SubCategoryDto>> GetSubCategoriesByCategoryIdAsync(Guid categoryId)
        {
            //bir üst kategoriye ait alt kategorileri listele.
            var subCategories = await _subCategoryRepo.FindAsync(c => c.CategoryId == categoryId);
            return _mapper.Map<IEnumerable<SubCategoryDto>>(subCategories);

        }

        public async Task<SubCategoryDto> GetSubcategoryByIdAsync(Guid id)
        {
            var subcategory = await _subCategoryRepo.GetByIdAsync(id);
            if (subcategory == null)
            {
                return null;
            }
            return _mapper.Map<SubCategoryDto>(subcategory);
        }

        public async Task<(bool succeeded, IEnumerable<string> errors)> UpdateCategoryAsync(EditCategoryDto editCategoryDto)
        {
            var category = await _categoryRepo.GetByIdAsync(editCategoryDto.Id);//güncellenecek kategorinin id'si. GetByIdAsync metodu Guid tipinde nesne alır. 
            if (category == null)
            {
                return (false, new[] {"Güncellenecek kategori bulunamadı."});
            }
            //aynı slug'a sahip başka bir kategori mevcut mu?
            if (category.Slug != editCategoryDto.Slug)
            {
                var existingCategory = await _categoryRepo.GetFirstOrDefaultAsync(c => c.Slug == editCategoryDto.Slug);
                if (existingCategory != null)
                {
                    return (false, new[] {"Bu slug'a sahip bir kategori zaten mevcut"});
                }
            }
            _mapper.Map(editCategoryDto, category);
            _categoryRepo.Update(category);
            await _categoryRepo.SaveChangesAync();
            return (true, Enumerable.Empty<string>());
        }

        public async Task<(bool succeeded, IEnumerable<string> errors)> UpdateSubCategoryAsync(EditSubCategoryDto editSubCategoryDto)
        {
            var subcategory = await _subCategoryRepo.GetByIdAsync(editSubCategoryDto.Id);//güncellenecek kategorinin id'si
            if (subcategory == null)
            {
                return (false, new[] { "Güncellenecek kategori bulunamadı." });
            }
            //aynı slug'a sahip başka bir kategori mevcut mu?
            if (subcategory.Slug != editSubCategoryDto.Slug)
            {
                var existingSubCategory = await _categoryRepo.GetFirstOrDefaultAsync(c => c.Slug == editSubCategoryDto.Slug);
                if (existingSubCategory != null)
                {
                    return (false, new[] { "Bu slug'a sahip bir kategori zaten mevcut" });
                }
            }
            _mapper.Map(editSubCategoryDto, subcategory);
            _subCategoryRepo.Update(subcategory);
            await _categoryRepo.SaveChangesAync();
            return (true, Enumerable.Empty<string>());
        }
    }
}
