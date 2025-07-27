using eCommerceApp.Application.DTOs.User;
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


        public async Task<IEnumerable<UserDto>> GetAllActiveUsersAsync()
        {
            //Tüm kullanıcıları repository'den çekiyoruz.
            //AppUser entity'den UserDto'ya dönüşüm burada yapılır.
            var users = await _userRepository.FindAsync(u => u.IsDeleted == false);
            var userDtos = new List<UserDto>();
            foreach (var user in users)
            {
                //her kullanıcının rol bilgisini çekiyoruz.
                var roles = await _userManager.GetRolesAsync(user);

                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    Fullname = user.Fullname ?? "N/A", //null kontrolü
                    Email = user.Email,
                    Bio = user.Bio,
                    ProfilImgUrl = user.ProfilImgUrl,
                    Location = user.Location,
                    IsActive = user.IsActive,
                    LastLoginDate = user.LastLoginDate,
                    RegistrationDate = user.RegistrationDate,
                    Roles = roles.ToList()//rolleri dto'ya ekliyoruz.
                });
            }
            return userDtos;
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
                ProfilImgUrl = createUserDto.ProfilImgUrl,
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

            await _userRepository.SaveChangesAync();//kaydı veritabanına yansıt
            return (true, Enumerable.Empty<string>());
        }

        public async Task<(bool Succeeded, IEnumerable<string> Errors)> UpdateUserAsync(EditUserDto editUserDto)
        {
            //Kullanıcıyı ID'sine göre bul.
            var user = await _userRepository.GetByIdAsync(editUserDto.Id);
            if (user == null)
            {
                return (false, new[] { "Güncelenecek kullanıcı bulunamadı." });
            }
            //e-posta değiştirme kontrolü.
            if (user.Email != editUserDto.Email)
            {
                //Identity UserManager üzerinden email'i güvenli bir şekilde güncelle
                var setEmailresult = await _userManager.SetEmailAsync(user, editUserDto.Email);
                if (!setEmailresult.Succeeded)
                {
                    return (false, setEmailresult.Errors.Select(e=>e.Description));
                }
                //email güncelle
                user.UserName = editUserDto.Email;
                user.NormalizedUserName = _userManager.NormalizeName(editUserDto.Email);
                user.NormalizedEmail = _userManager.NormalizeEmail(editUserDto.Email);
                user.EmailConfirmed = false;//email değiştiyse e-posta onayı gerekbilir.
            }
            //Diğer profil bilgilerini EditUserDto'dan AppUser entity'sine aktar.
            user.Fullname = editUserDto.Fullname;
            user.Bio = editUserDto.Bio;
            user.ProfilImgUrl = editUserDto.ProfilImgUrl;
            user.Location = editUserDto.Location;
            user.IsActive = editUserDto.IsActive;//Aktif-Pasif olma durumunu güncelleyebilir.
            user.ModifiedDate = DateTime.UtcNow;//son güncellenme tarihini al

            // kullanıcı nesnesindeki değişiklikleri Identity UserManager üzerinden güncelle
            // Bu , ıdentity'nin internal mekanizmalarını da çalışmasını sağlar.
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return (false, updateResult.Errors.Select(e => e.Description));
            }
            // değişiklikleri veritabanına kaydet.
            await _userRepository.SaveChangesAync();
            return (true, Enumerable.Empty<string>());
        }

        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                Id = user.Id,
                Fullname = user.Fullname ?? "N/A",
                Email = user.Email ?? "N/A",
                Bio = user.Bio,
                ProfilImgUrl = user.ProfilImgUrl,
                Location= user.Location,
                IsActive = user.IsActive,
                LastLoginDate = user.LastLoginDate,
                RegistrationDate = user.RegistrationDate,
                Roles = roles.ToList()
            };
        }

        // TÜM SOFT-SİLİNMİŞ KULLANICILARI GETİRME METODU
        public async Task<IEnumerable<UserDto>> GetAllDeletedUsersAsync()
        {
            // Sadece silinmiş (IsDeleted == true) kullanıcıları getiriyoruz
            var users = await _userRepository.FindAsync(u => u.IsDeleted == true);

            var userDtos = new List<UserDto>();
            foreach (var user in users)
            {
                // Silinmiş kullanıcıların rollerini de çekiyoruz
                var roles = await _userManager.GetRolesAsync(user);

                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    Fullname = user.Fullname ?? "N/A",
                    Email = user.Email ?? "N/A",
                    Bio = user.Bio,
                    ProfilImgUrl = user.ProfilImgUrl,
                    Location = user.Location,
                    IsActive = user.IsActive,
                    LastLoginDate = user.LastLoginDate,
                    RegistrationDate = user.RegistrationDate,
                    Roles = roles.ToList(),
                    IsDeleted = user.IsDeleted,
                    DeletedDate = user.DeletedDate, // DeletedDate'i DTO'ya eklemiştik
                    DeletedBy = user.DeletedBy
                });
            }
            return userDtos;
        }

        // KULLANICIYI GERİ YÜKLEME METODU
        public async Task<(bool Succeeded, IEnumerable<string> Errors)> RestoreUserAsync(string userId, string restoredBy)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return (false, new[] { "Geri yüklenecek kullanıcı bulunamadı." });
            }

            if (!user.IsDeleted) // Eğer zaten silinmemişse geri yüklemeye çalışma
            {
                return (false, new[] { "Kullanıcı zaten silinmiş durumda değil." });
            }

            user.IsDeleted = false; // Silindi bayrağını kaldır
            user.IsActive = true;   // Kullanıcıyı tekrar aktif yap
            user.DeletedDate = null; // Silinme tarihini sıfırla
            user.DeletedBy = null;   // Kimin sildiğini sıfırla
            user.ModifiedDate = DateTime.UtcNow; // Güncelleme tarihini işaretle
            user.ModifiedBy = restoredBy; // Kimin geri yüklediğini de kaydedelim (isteğe bağlı)

            _userRepository.Update(user); // EF Core değişikliği izleyecek

            // UserManager üzerinden güncelleme, Identity'nin internal state'ini de günceller
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return (false, updateResult.Errors.Select(e => e.Description));
            }

            await _userRepository.SaveChangesAync(); // Değişiklikleri veritabanına kaydet

            return (true, Enumerable.Empty<string>());
        }

        // KULLANICIYI SOFT DELETE İLE SİLME METODU
        public async Task<(bool Succeeded, IEnumerable<string> Errors)> SoftDeleteUserAsync(string userId, string deletedBy)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return (false, new[] { "Silinecek kullanıcı bulunamadı." });
            }

            user.IsDeleted = true;
            user.IsActive = false; // Silinen kullanıcıyı aynı zamanda pasif yap
            user.DeletedDate = DateTime.UtcNow;
            user.DeletedBy = deletedBy;
            user.ModifiedDate = DateTime.UtcNow; // Güncelleme tarihini de işaretle

            _userRepository.Update(user); // EF Core değişikliği izleyecektir

            var updateResult = await _userManager.UpdateAsync(user); // Identity'nin de güncellemeyi tanıması için
            if (!updateResult.Succeeded)
            {
                return (false, updateResult.Errors.Select(e => e.Description));
            }

            await _userRepository.SaveChangesAync(); // Değişiklikleri kaydet

            return (true, Enumerable.Empty<string>());
        }
    }
}
