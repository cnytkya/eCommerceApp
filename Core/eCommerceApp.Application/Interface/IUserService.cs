
using eCommerceApp.Application.Dtos.User;
using eCommerceApp.Application.DTOs.User;

namespace eCommerceApp.Application.Interface
{
    public interface IUserService
    {
        //aktif olan bütün kullanıcıları listelemek için
        Task<UserDto> GetUserByIdAsync(string id);
        Task<IEnumerable<UserDto>> GetAllActiveUsersAsync();

        //yeni user ekleme metodu.
        Task<(bool Succeeded, IEnumerable<string> Errors)> CreateUserAsync(CreateUserDto createUserDto, string roleName);

        //user güncelleme metodu
        Task<(bool Succeeded, IEnumerable<string> Errors)> UpdateUserAsync(EditUserDto editUserDto);

    }
}
