using eCommerceApp.Application.Interface;
using eCommerceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApp.Infrastructure.Persistence.Repositories
{
    public class UserRepository : Repository<AppUser>, IUserRepository
    {
        public UserRepository( AppDbContext context) : base(context)
        {
        }

        public async Task<AppUser> GetUserWithRoleAsync(string userId)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<IEnumerable<AppUser>> SearchUsersByFullNameAsync(string fullname)
        {
            if (string.IsNullOrWhiteSpace(fullname))
            {
                return Enumerable.Empty<AppUser>();
            }
            return await _dbSet.Where(u => u.Fullname != null && u.Fullname.Contains(fullname)).ToListAsync();
        }
    }
}
