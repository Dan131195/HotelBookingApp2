using System.ComponentModel.DataAnnotations;

namespace HotelBookingApp2.Models
{
    public class Camera
    {
        [Key]
        public Guid CameraId { get; set; } = Guid.NewGuid();

        [Required]
        public int Numero { get; set; }

        [Required]
        public string Tipo { get; set; }

        [Required]
        public decimal Prezzo { get; set; }

        public bool Disponibilita { get; set; }

        public ICollection<Prenotazione> Prenotazioni { get; set; }
    }
}
