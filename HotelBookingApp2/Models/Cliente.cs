using System.ComponentModel.DataAnnotations;

namespace HotelBookingApp2.Models
{
    public class Cliente
    {
        [Key]
        public Guid ClienteId { get; set; } = Guid.NewGuid();

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Cognome { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string Telefono { get; set; }

        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }

        public ICollection<Prenotazione> Prenotazioni { get; set; }
    }
}
