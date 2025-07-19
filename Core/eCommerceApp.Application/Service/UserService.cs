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

        public async Task<(bool Succeeded, IEnumerable<string> Errors)> CreateUserAsync(CreateUserDto createUserDto, string roleName)
        {
            //email zaten kullanımda mı
            var existingUser = await _userManager.FindByEmailAsync(createUserDto.Email);
            //kullanıcı mevcut ise iş kuralını yazalım
            if (existingUser != null)
            {
                return (false, new[] { "Bu email zaten kullanımda!" });
            }

            var user = new AppUser
            {
                Fullname = createUserDto.Fullname,
                Email = createUserDto.Email,
                UserName = createUserDto.Email,
                Bio = createUserDto.Bio,
                ProfilImgUrl = createUserDto.ImgUrl,
                IsActive = createUserDto.IsActive,//cretauserdto'dan IsActive bilgisini alıyoruz.
                RegistrationDate = DateTime.Now,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
            };

            //Şifreyi CreateUserDto'dan alıyoruz.
            var creatResult = await _userManager.CreateAsync(user, createUserDto.Password);
            if (!creatResult.Succeeded)
            {
                return (false, creatResult.Errors.Select(e => e.Description));
            }

            //Rolün var olduğundan emin olmamız lazım. İlk defa oluşuyorsa default olarak User atanacak(Register tarafında). Admin tarafında oluşturulacaksa admin istediği rolü atayabilir.
            if (!await _roleManager.RoleExistsAsync(roleName))//eğer rol smevcut değilse
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            var roleResult = await _userManager.AddToRoleAsync(user, roleName);

            if (!roleResult.Succeeded)
            {
                //Rol ataması başarısız olursa kullanıcıyı geri almayı düşün.
                await _userManager.DeleteAsync(user);//Kullanıcıyı sil.
                return (false, roleResult.Errors.Select(e => e.Description).Append("Kullanıcı oluşturuldu ancak rol ataması başarısız oldu ve kullanıcı silindi."));
            }

            await _userRepository.SaveChangesAync();
            return (true, Enumerable.Empty<string>());
        }

        public async Task<IEnumerable<UserListDto>> GetAllActiveUsersAsync()
        {
            //Tüm kullanıcıları repository'den çekiyoruz.
            //AppUser entity'den UserDto'ya dönüşüm burada yapılır.
            var users = await _userRepository.FindAsync(u => u.IsDeleted == false);
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
