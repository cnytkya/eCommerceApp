using Microsoft.AspNetCore.Identity;

namespace eCommerceApp.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string? Fullname { get; set; }

        //Profile information
        public string? Bio { get; set; }
        public string? ProfilImgUrl { get; set; }
        public string? Location { get; set; }
        
        //User status and activity tracking
        public bool IsActive { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        //Navigation propperties
        //public ICollection<Message> Messages { get; set; }
        //public ICollection<Comment> Comments { get; set; }

        //Auidit fields
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string? ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

        //Soft delete
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; } = DateTime.UtcNow;
        public string? DeletedBy { get; set; }
    }
}
