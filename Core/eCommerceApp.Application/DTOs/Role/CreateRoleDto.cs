using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Application.DTOs.Role
{
    public class CreateRoleDto //Index için.
    {
        [Required(ErrorMessage = "Rol adı boş bırakılamaz")]
        [StringLength(100,ErrorMessage = "Rol adı en fazla {1} karakter olmalı.")]
        [Display(Name = "Rol Adı")]
        public string Name { get; set; }
    }
}
