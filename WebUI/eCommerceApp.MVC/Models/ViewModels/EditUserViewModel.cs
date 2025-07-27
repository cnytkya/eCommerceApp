using eCommerceApp.Application.DTOs.User;
using Microsoft.AspNetCore.Mvc.Rendering; // SelectListItem için
using System.ComponentModel.DataAnnotations; // Display için

namespace eCommerceApp.MVC.Models.ViewModels
{
    // EditUserDto'dan kalıtım alarak temel kullanıcı bilgilerini miras alıyoruz
    public class EditUserViewModel : EditUserDto
    {
        // UI'a özgü ek alanlar ve roller
        [Display(Name = "Mevcut Roller")]
        public List<string> CurrentRoles { get; set; } = new List<string>(); // Kullanıcının şu anki rolleri (sadece gösterim için)

        [Display(Name = "Atanacak Roller")]
        public List<string> SelectedRoles { get; set; } = new List<string>(); // Formdan seçilen yeni roller (POST için)

        public IEnumerable<SelectListItem>? AllRoles { get; set; } // Dropdown için tüm roller (GET için)
    }
}