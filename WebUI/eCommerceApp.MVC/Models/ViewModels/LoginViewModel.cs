using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.MVC.Models.ViewModels
{
	public class LoginViewModel
	{
		[Required(ErrorMessage ="Email alanı boş bırakılamaz!")]
		[EmailAddress(ErrorMessage="Geçerli bir email adresi giriniz.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Şifre alanı boş bırakılamaz!")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Display(Name ="Beni hatırla")]
		public bool RememberMe { get; set; }
		public string? ReturnUrl { get; set; } //Kullanıcıdan girişten sonrası yönlendirme.
	}
}
