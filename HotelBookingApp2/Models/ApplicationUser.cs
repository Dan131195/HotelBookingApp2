using Microsoft.AspNetCore.Identity;

namespace HotelBookingApp2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public ICollection<Prenotazione> Prenotazioni { get; set; }
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
