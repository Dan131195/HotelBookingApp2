using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingApp2.ViewModels
{
    public class PrenotazioneViewModel
    {
        public Guid PrenotazioneId { get; set; }

        [Required(ErrorMessage = "Il cliente è obbligatorio")]
        public Guid ClienteId { get; set; }

        [Required(ErrorMessage = "La camera è obbligatoria")]
        public Guid CameraId { get; set; }

        [Required(ErrorMessage = "La data di inizio è obbligatoria")]
        [DataType(DataType.Date)]
        public DateTime DataInizio { get; set; }

        [Required(ErrorMessage = "La data di fine è obbligatoria")]
        [DataType(DataType.Date)]
        public DateTime DataFine { get; set; }

        [Required(ErrorMessage = "Lo stato è obbligatorio")]
        public string Stato { get; set; } = string.Empty;

        public SelectList? Clienti { get; set; }
        public SelectList? Camere { get; set; }
    }
}
