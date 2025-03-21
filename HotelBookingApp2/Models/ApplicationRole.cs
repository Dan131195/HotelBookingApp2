using Microsoft.AspNetCore.Identity;

namespace HotelBookingApp2.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
