using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Application.DTOs.Role
{
    public class RoleDto //Index için.
    {
        public string Id { get; set; } //Benzersiz Id
        [Required(ErrorMessage = "Rol adı bo bırakılamaz")]
        [StringLength(100,ErrorMessage = "Rol adı en fazla {1} karakter olmalı.")]
        [Display(Name = "Rol Adı")]
        public string Name { get; set; }
        [Display(Name = "Kullanıcı Sayısı")]
        public int Count { get; set; }
    }
    //public class CreateRoleDto //Bu DTO'yu Role klasörüne otomatik atması için CreateRoleDto üzerine sağ click, ampülden Quick Actions...dan Move type to ...
    //{
    //    [Required(ErrorMessage = "Rol adı bo bırakılamaz")]
    //    [StringLength(100,ErrorMessage = "Rol adı en fazla {1} karakter olmalı.")]
    //    [Display(Name = "Rol Adı")]
    //    public string Name { get; set; }
    //}
}
