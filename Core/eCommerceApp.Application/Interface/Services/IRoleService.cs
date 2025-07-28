using eCommerceApp.Application.DTOs.Role;

namespace eCommerceApp.Application.Interface.Services
{
    public interface IRoleService
    {
        //Bütün rolleri listele
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        //Id'ye göre rol detay bilgilerini getir
        Task<RoleDto?> GetRoleByIdAsync(string id);
        //Yeni rol oluşturma
        Task<(bool Succeeded, IEnumerable<string> Errors)> CreateRoleAsync(CreateRoleDto createRoleDto);
        //Mevcut rolü güncelleme
        Task<(bool Succeeded, IEnumerable<string> Errors)> UpdateRoleAsync(EditRoleDto editRoleDto);
        //Rol silme
        Task<(bool Succeeded, IEnumerable<string> Errors)> DeleteRoleAsync(string roleId);
        //Belli bir kullanıcıya rol atama
        Task<(bool Succeeded, IEnumerable<string> Errors)> AssignRoleToUserAsync(string id, string roleName);
        //Belli bir kullanıcıdan rolü kaldırma
        Task<(bool Succeeded, IEnumerable<string> Errors)> RemoveRoleFromUserAsync(string id, string roleName);

        //Bir kullanıcıya ait rolleri listeleme
        Task<IList<string>> GetUserRolesAsync(string id);
    }
}
