using eCommerceApp.Application.Interface.Repositories.Categories;
using eCommerceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApp.Infrastructure.Persistence.Repositories.Categories
{
    public class SubCategoryRepo : Repository<Subcategory>, ISubCategoryRepo
    {
        public SubCategoryRepo(AppDbContext context) : base(context)
        {
        }
    }
}
