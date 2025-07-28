using eCommerceApp.Application.DTOs.Role;
using eCommerceApp.Application.Interface.Services;
using eCommerceApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApp.Application.Service
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<(bool Succeeded, IEnumerable<string> Errors)> AssignRoleToUserAsync(string id, string roleName)
        {
            //Kullanıcıya rol atamak için kullancıyı Id'ye göre getir.
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return (false, new[] {"Kullanıcı bulunamadı"});
            }
            //Belirtilen role mevcut mu kontrol et.
            if (!await _roleManager.RoleExistsAsync(roleName))//Mevcut değilse
            {
                return (false, new[] { "Belirtilen rol mevcut değil." });
            }
            //Role mevcut ise seçilen rolü kullanıcıya ata.
            var roleResult = await _userManager.AddToRoleAsync(user, roleName);
            return (roleResult.Succeeded, roleResult.Errors.Select(e=>e.Description));
        }

        public async Task<(bool Succeeded, IEnumerable<string> Errors)> CreateRoleAsync(CreateRoleDto createRoleDto)
        {
            //Zaten rol var mı
            if (await _roleManager.RoleExistsAsync(createRoleDto.Name))
            {
                return (false, new[] { "Bu rol adı zaten var." });
            }
            //Role mevcut değilse bunu dto'ya aktaralım
            var role = new IdentityRole(createRoleDto.Name);
            var result = await _roleManager.CreateAsync(role);
            return (result.Succeeded, result.Errors.Select(e => e.Description));
        }

        public async Task<(bool Succeeded, IEnumerable<string> Errors)> DeleteRoleAsync(string roleId)
        {
            //Rolü silmek üzere rolü Id'ye göre getir.
            var role = await _roleManager.FindByIdAsync(roleId);
            //Eğer roleId boş ise
            if (role == null)
            {
                return (false, new[] { "Rol bulunamadı" });
            }
            //Eğer bu rolde kullanıcı veya kullanıcılar varsa uyaralım.
            var userInRole = await _userManager.GetUsersInRoleAsync(role.Name);
            if (userInRole.Any())
            {
                return (false, new[] { $"Bu role sahip {userInRole.Count} kullanıcı var. Rolü silmeden önce kullanıcıların rolünü güncelleyebilirsiniz." });
            }
            //silmeyi gerçekleştir.
            var result = await _roleManager.DeleteAsync(role);
            return (result.Succeeded, result.Errors.Select(e => e.Description));
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            //Bütün rolleri veritabanından getir
            var roles = await _roleManager.Roles.ToListAsync();
            //roleDtos listesi oluştur.
            var roleDtos = new List<RoleDto>();
            foreach (var role in roles)
            {
                var userInRole = await _userManager.GetUsersInRoleAsync(role.Name);
                roleDtos.Add(new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    Count = userInRole.Count,//Bir role sahip kullanıcı sayısı
                });
            }
            return roleDtos;
        }

        public async Task<RoleDto?> GetRoleByIdAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return null;

            var userInRole = await _userManager.GetUsersInRoleAsync(role.Name);
            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Count = userInRole.Count,//Bir role sahip kullanıcı sayısı
            };
        }

        public Task<IList<string>> GetUserRolesAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<(bool Succeeded, IEnumerable<string> Errors)> RemoveRoleFromUserAsync(string id, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<(bool Succeeded, IEnumerable<string> Errors)> UpdateRoleAsync(EditRoleDto editRoleDto)
        {
            throw new NotImplementedException();
        }
    }
}
