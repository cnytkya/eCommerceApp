using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceApp.Application.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(string id);//IdentityUser'lar string ID kullanır
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);//Belirtilen koşula uyan ilk varlığı asenkron olarak getir.
        Task<IEnumerable<T>> GetAllAsync();
    }
}
