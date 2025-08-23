using eCommerceApp.Domain.Entities;

namespace eCommerceApp.Application.Interface.Repositories.Categories
{
    public interface ICategoryRepo : IRepository<Category>
    {
        Task<Category?> GetCategoryBySlugAsync(string slug);
    }
}
