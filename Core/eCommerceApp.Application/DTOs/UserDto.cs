using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Application.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }
        [Display(Name ="Kullanıcı Adı"),Required(ErrorMessage ="Fullname alanı boş bırakılamaz")]
        [StringLength(100, ErrorMessage = "Kullanıcı adı en fazla 100 karakter uzunluğunda olmalıdır.")]
        public string FullName { get; set; }
        [Display(Name = "Email Adresi"),Required(ErrorMessage ="Email alanı boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        public string Email { get; set; }
        [Display(Name = "Biyografi")]
        [StringLength(500, ErrorMessage = "Biyografi en fazla {1}  karakter uzunluğunda olmalıdır.")]
        public string? Bio { get; set; }
        [Display(Name = "Resim")]
        [Url(ErrorMessage = "Geçerli bir URL giriniz")]
        public string? ImgUrl { get; set; }
        [Display(Name = "Konum")]
        [StringLength(100, ErrorMessage = "Konum en fazla {1}  karakter uzunluğunda olmalıdır.")]
        public string? Location { get; set; }
        [Display(Name = "Aktif Mi?")]
        public bool IsActive { get; set; }
        [Display(Name = "Son Giriş Tarihi")]
        public DateTime LastLoginDate { get; set; }
        //[Display(Name = "Kayıt Tarihi")]
        public DateTime RegistrationDate { get; set; }
        public IList<string> Roles { get; set; }
    }
}
