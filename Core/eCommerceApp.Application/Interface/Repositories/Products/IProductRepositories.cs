using eCommerceApp.Domain.Entities;

namespace eCommerceApp.Application.Interface.Repositories.Products
{
    public interface IProductRepositories : IRepository<Product>
    {
        //İleriki aşamalarda product modeline özel metotlar burda tanımlanabilir.
        //Örneğin fiyata veya stoğa göre ürün araması.
    }
}
