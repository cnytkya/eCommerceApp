using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Application.Dtos.User
{
    public class CreateUserDto
    {
        [StringLength(100, ErrorMessage = "Adı Soyadı max 100 karakterden uzun olamaz.")]
        [Display(Name = "Adı Soyadı"), Required(ErrorMessage = "Adı Soyadı boş bırakılamaz.")]
        public string Fullname { get; set; }

        [EmailAddress(ErrorMessage = "Geçerli bir email adres giriniz.")]
        [Display(Name = "Email"), Required(ErrorMessage = "Email alanı boş bırakılamaz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")]
        [StringLength(100, ErrorMessage = "Şifre en az 6 max 100 karakterden uzunluğunda olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //Password fields
        [Display(Name = "Şifre tekrar")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Biyografi")]
        [StringLength(500, ErrorMessage = "Biyografi max 500 karakterden uzun olamaz.")]
        public string? Bio { get; set; }
        [Display(Name = "Profile Resmi URL")]

        public string? ImgUrl { get; set; }

        [Display(Name = "Konum Bilgisi")]
        [StringLength(200, ErrorMessage = "Konum bilgisi max 100 karakterden uzun olamaz.")]
        public string? Location { get; set; }

        [Display(Name = "Aktif Mi?")]
        public bool IsActive { get; set; } = true; //varsayılan aktif


    }
}
