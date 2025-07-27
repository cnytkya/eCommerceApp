using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Application.DTOs.User
{
    public class EditUserDto
    {
        [Required(ErrorMessage = "Id alanı zorunlu.")]
        public string Id { get; set; }//Hangi kullanıcının güncelledniğini belirtmek için ID

        [StringLength(100, ErrorMessage = "Adı soyadı max 100 karakter olabilir")]
        [Display(Name = "Adı Soyadı"), Required(ErrorMessage = "Adı soyadı boş bırakılamaz.")]
        public string Fullname { get; set; }

        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz!")]
        [Display(Name = "Email"), Required(ErrorMessage = "Email boş bırakılamaz.")]
        public string Email { get; set; }

        [Display(Name = "Biyografi")]
        [StringLength(500, ErrorMessage = "Biyografi en fazla 500 karakter uzunluğunda olmalı")]
        public string? Bio { get; set; }
        [Display(Name = "Profile Resmi URL")]
        public string? ImgUrl { get; set; }

        [Display(Name = "Konum Bilgisi")]
        [StringLength(100, ErrorMessage = "Konum en fazla 100 karakter uzunluğunda olmalı.")]
        public string? Location { get; set; }

        [Display(Name = "Aktif Mi?")]
        public bool IsActive { get; set; } = true; //varsayılan aktif.
    }
}
