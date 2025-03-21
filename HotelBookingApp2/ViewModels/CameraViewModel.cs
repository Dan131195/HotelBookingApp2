using System.ComponentModel.DataAnnotations;

namespace HotelBookingApp2.ViewModels
{
    public class CameraViewModel
    {
        public Guid? CameraId { get; set; }

        [Required(ErrorMessage = "Il numero è obbligatorio.")]
        public int Numero { get; set; }

        [Required(ErrorMessage = "Il tipo è obbligatorio.")]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "Il prezzo è obbligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Il prezzo deve essere maggiore di zero.")]
        public decimal Prezzo { get; set; }

        public bool Disponibilita { get; set; }
    }
}
