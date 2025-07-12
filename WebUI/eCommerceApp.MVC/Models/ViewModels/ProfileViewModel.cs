using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.MVC.Models.ViewModels
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Adı Soyadı alanı boş bırakılamaz.")]
        [Display(Name = "Adı Soyadı")]
        public string Fullname { get; set; }

        [Required(ErrorMessage ="Email alanı boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        [Display(Name = "Email Adresi")]
        public string Email { get; set; }

        [Display(Name = "Biyografi")]
        public string? Bio { get; set; }

        [Display(Name = "Profil Resmi")]
        public string? ProfileImgUrl { get; set; }

        [Display(Name = "Konum")]
        [StringLength(100, ErrorMessage = "Konum en fazla {1} karakter uzunluğunda olmalıdır.")]
        public string? Location { get; set; }

        //Şifre değiştirme alanları
        [DataType(DataType.Password)]
        [Display(Name = "Mevcut Şifre")]
        public string? OldPassword { get; set; }

        [StringLength(100, ErrorMessage = "Şifre en az 6 ve en fazla 100 karakter olmalıdır",MinimumLength =6)]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre Tekrar")]
        [Compare("NewPassword", ErrorMessage = "Yeni şifre ve onay şifresi uyuşmuyor")]
        public string? ConfirmNewPassword { get; set; }

    }
}
