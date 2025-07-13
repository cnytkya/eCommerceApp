using System.Linq.Expressions;

namespace eCommerceApp.Application.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(string id);//IdentityUser'lar string ID kullanır
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);//Belirtilen koşula uyan ilk varlığı asenkron olarak getir.
        Task<IEnumerable<T>> GetAllAsync();
        //Belirtilen koşula uyan varlıkları asenkron olarak getirir.
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    }
}
