using eCommerceApp.Domain.Entities;

namespace eCommerceApp.Application.Interface.Repositories.Categories
{
    public interface ICategoryRepo : IRepository<Category>
    {
        Task<Category?> GetCategoryBySlugAsync(string slug);

        //Alt kategorilerle birlikte tüm kategorileri asnekron olarak getir.
        Task<IEnumerable<Category>> GetAllCategoriesWithSubcategoriesAsync();//Include method(bir nesneyi çağırırken onunla ilişkili nesnenin herhangi bir özelliğin view etmek istersek Inlude etmemiz gerekir.)
    }
}
