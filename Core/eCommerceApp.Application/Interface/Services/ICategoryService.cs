using eCommerceApp.Application.DTOs.Category;
using eCommerceApp.Application.DTOs.Subcategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        #endregion
    }
}
