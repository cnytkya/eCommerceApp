using eCommerceApp.Domain.Entities;

namespace eCommerceApp.Application.Interface
{
    public interface IUserRepository : IRepository<AppUser>
    {
        Task<AppUser> GetUserWithRoleAsync(string userId);//Role ile birlikte kullanıcıyı ya da kullanıcıları getir.
        Task<IEnumerable<AppUser>> SearchUsersByFullNameAsync(string fullname);//İsme göre kullanıcı getir.
    }
}
