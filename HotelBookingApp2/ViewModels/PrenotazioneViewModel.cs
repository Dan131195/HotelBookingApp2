using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingApp2.ViewModels
{
    public class PrenotazioneViewModel
    {
        public Guid PrenotazioneId { get; set; }

        [Required]
        public Guid ClienteId { get; set; }

        [Required]
        public Guid CameraId { get; set; }

        [Required]
        public DateTime DataInizio { get; set; }

        [Required]
        public DateTime DataFine { get; set; }

        [Required]
        public string Stato { get; set; }

        public SelectList? Clienti { get; set; }
        public SelectList? Camere { get; set; }
    }
}