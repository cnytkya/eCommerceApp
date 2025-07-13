using eCommerceApp.Application.DTOs;
using eCommerceApp.Application.Interface;
using eCommerceApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace eCommerceApp.Application.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(IUserRepository userRepository, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<UserListDto>> GetAllActiveUsersAsync()
        {
            //Tüm kullanıcıları repository'den çekiyoruz.
            //AppUser entity'den UserDto'ya dönüşüm burada yapılır.
            var users = await _userRepository.FindAsync(u=>u.IsDeleted == false);
            var userDtos = new List<UserListDto>();
            foreach (var user in users)
            {
                //her kullanıcının rol bilgisini çekiyoruz.
                var roles = await _userManager.GetRolesAsync(user);

                userDtos.Add(new UserListDto
                {
                    Id = user.Id,
                    FullName = user.Fullname ?? "N/A", //null kontrolü
                    Email = user.Email,
                    Bio = user.Bio,
                    ImgUrl = user.ProfilImgUrl,
                    Location = user.Location,
                    IsActive = user.IsActive,
                    LastLoginDate = user.LastLoginDate,
                    RegistrationDate = user.RegistrationDate,
                    Roles = roles.ToList()//rolleri dto'ya ekliyoruz.
                });
            }
            return userDtos;
        }
    }
}
