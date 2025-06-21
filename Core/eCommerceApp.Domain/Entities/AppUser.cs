using Microsoft.AspNetCore.Identity;

namespace eCommerceApp.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
