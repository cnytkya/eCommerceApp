using eCommerceApp.Application.DTOs;

namespace eCommerceApp.Application.Interface
{
    public interface IUserService
    {
        //aktif olan bütün kullanıcıları listelemek için
        Task<IEnumerable<UserListDto>> GetAllActiveUsersAsync();

        //yeni user ekleme metodu.
        Task<(bool Succeeded, IEnumerable<string> Errors)> CreateUserAsync(CreateUserDto createUserDto, string roleName);
    }
}
