using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.MVC.Models
{
public class RegisterViewModel
{
	[Required(ErrorMessage = "Ad Soyad alanı boş bırakılamaz.")]
	[StringLength(100, ErrorMessage = "Ad Soyad alanı en fazla 100 karakter olabilir.")]
	public string FullName { get; set; }

	[Required(ErrorMessage = "Email alanı boş bırakılamaz.")]
	[EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
	public string Email { get; set; }

	[Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")]
	[StringLength(100, ErrorMessage = "{0} en az {2} ve en fazla {1} karakter uzunluğunda olmalıdır.", MinimumLength = 6)]
	[DataType(DataType.Password)]
	[Display(Name = "Şifre")]
	public string Password { get; set; }

	[DataType(DataType.Password)]
	[Display(Name = "Şifre Tekrar")]
	[Compare("Password", ErrorMessage = "Şifre ve şifre tekrarı eşleşmiyor.")]
	public string ConfirmPassword { get; set; }
}
}