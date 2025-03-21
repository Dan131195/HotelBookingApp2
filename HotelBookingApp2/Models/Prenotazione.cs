using System.ComponentModel.DataAnnotations;

namespace HotelBookingApp2.Models
{
    public class Prenotazione
    {
        [Key]
        public Guid PrenotazioneId { get; set; } = Guid.NewGuid();

        public Guid ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public Guid CameraId { get; set; }
        public Camera Camera { get; set; }

        public DateTime DataInizio { get; set; }
        public DateTime DataFine { get; set; }

        public string Stato { get; set; }

        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
    }
}
