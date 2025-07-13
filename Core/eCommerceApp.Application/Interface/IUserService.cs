using eCommerceApp.Application.DTOs;

namespace eCommerceApp.Application.Interface
{
    public interface IUserService
    {
        //aktif olan bütün kullanıcıları listelemek için
        Task<IEnumerable<UserListDto>> GetAllActiveUsersAsync();
    }
}
