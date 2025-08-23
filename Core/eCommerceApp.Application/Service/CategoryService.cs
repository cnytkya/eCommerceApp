using eCommerceApp.Application.DTOs.Category;
using eCommerceApp.Application.DTOs.Subcategory;
using eCommerceApp.Application.Interface.Services;

namespace eCommerceApp.Application.Service
{
    public class CategoryService : ICategoryService
    {

        //DI
        public Task<(bool succeeded, IEnumerable<string> errors)> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            throw new NotImplementedException();
        }

        public Task<(bool succeeded, IEnumerable<string> errors)> CreateSubCategoryAsync(CreateSubCategoryDto createSubCategoryDto)
        {
            throw new NotImplementedException();
        }

        public Task<(bool succeeded, IEnumerable<string> errors)> DeleteCategoryAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<(bool succeeded, IEnumerable<string> errors)> DeleteSubCategoryAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CategoryDto?> GetCategoryByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SubCategoryDto>> GetSubCategoriesByCategoryIdAsync(Guid categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<(bool succeeded, IEnumerable<string> errors)> UpdateCategoryAsync(EditCategoryDto editCategoryDto)
        {
            throw new NotImplementedException();
        }

        public Task<(bool succeeded, IEnumerable<string> errors)> UpdateSubCategoryAsync(EditSubCategoryDto editSubCategoryDto)
        {
            throw new NotImplementedException();
        }
    }
}
