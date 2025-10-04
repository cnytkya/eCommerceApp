using eCommerceApp.Application.DTOs.Category;
using eCommerceApp.Application.DTOs.Subcategory;

namespace eCommerceApp.Application.Interface.Services
{
    public interface ICategoryService
    {

        //category CRDU operartions
        #region category
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(Guid id);
        Task<(bool succeeded, IEnumerable<string> errors)> CreateCategoryAsync(CreateCategoryDto createCategoryDto);
        Task<(bool succeeded, IEnumerable<string> errors)> UpdateCategoryAsync(EditCategoryDto editCategoryDto);
        Task<(bool succeeded, IEnumerable<string> errors)> DeleteCategoryAsync(Guid id);
        #endregion

        //sub category CRUD operartions
        #region subcategory
        Task<IEnumerable<SubCategoryDto>> GetSubCategoriesByCategoryIdAsync(Guid categoryId);
        Task<(bool succeeded, IEnumerable<string> errors)> CreateSubCategoryAsync(CreateSubCategoryDto createSubCategoryDto);
        Task<(bool succeeded, IEnumerable<string> errors)> UpdateSubCategoryAsync(EditSubCategoryDto editSubCategoryDto);
        Task<(bool succeeded, IEnumerable<string> errors)> DeleteSubCategoryAsync(Guid id);

        Task<SubCategoryDto> GetSubcategoryByIdAsync(Guid id);//alt kategoriyi id'ye göre çekecek metot.

        //Yeni metot: kategori ve ürün sayısını gösterecek fonk.
        Task<IEnumerable<CategoryWithProductCountDto>> GetCategoriesWithProductCountsAsync();
        #endregion
    }
}
