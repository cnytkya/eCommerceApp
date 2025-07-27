using eCommerceApp.Application.DTOs.User;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.MVC.Models.ViewModels
{
    public class EditUserViewModel : EditUserDto
    {
        //Kullanıcının o anki rolü
        [Display(Name = "Mevcut Roller")]
        public List<string> CurrentRoles { get; set; }

        // Formdan seçeceği roller
        [Display(Name = "Atanacak Roller")]
        public List<string> SelectedRole { get; set; }

        // Dropdown içerisine bütün rolleri ekleyelim.
        public IEnumerable<SelectListItem> AllRoles { get; set; }

    }
}
