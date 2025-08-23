using System.Linq.Expressions;

namespace eCommerceApp.Application.Interface.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(string id);//IdentityUser'lar string ID kullanır
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);//Belirtilen koşula uyan ilk varlığı asenkron olarak getir.
        Task<IEnumerable<T>> GetAllAsync();
        //Belirtilen koşula uyan varlıkları asenkron olarak getirir.
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        //Write(yazma) methods
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);


        //kayıtları veritabanına uygula
        Task<int> SaveChangesAync();
    }
}
