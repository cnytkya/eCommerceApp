using eCommerceApp.Application.DTOs.User;

namespace eCommerceApp.Application.Interface.Services
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

        // KULLANICIYI SOFT DELETE İLE SİLMEK İÇİN
        Task<(bool Succeeded, IEnumerable<string> Errors)> SoftDeleteUserAsync(string userId, string deletedBy);
        // TÜM SOFT-SİLİNMİŞ KULLANICILARI LİSTELEMEK İÇİN
        Task<IEnumerable<UserDto>> GetAllDeletedUsersAsync();

        // SOFT-SİLİNMİŞ BİR KULLANICIYI GERİ YÜKLEMEK İÇİN
        Task<(bool Succeeded, IEnumerable<string> Errors)> RestoreUserAsync(string userId, string restoredBy);

    }
}
