using eCommerceApp.Application.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace eCommerceApp.Infrastructure.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _appDbContext;
        //Doğrudan erişim dbset kullanılabilir.
        protected readonly DbSet<T> _dbSet;

        public Repository(DbSet<T> dbSet, AppDbContext appDbContext)
        {
            _dbSet = dbSet;
            _appDbContext = appDbContext;
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
