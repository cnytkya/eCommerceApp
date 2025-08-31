using eCommerceApp.Application.Interface.Repositories.Products;
using eCommerceApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceApp.Infrastructure.Persistence.Repositories.Products
{
    public class ProductRepo : Repository<Product>, IProductRepositories
    {
        public ProductRepo(AppDbContext context) : base(context)
        {
        }
    }
}
