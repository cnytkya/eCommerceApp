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
        public async Task<T?> GetByIdAsync(string id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
    }
}
