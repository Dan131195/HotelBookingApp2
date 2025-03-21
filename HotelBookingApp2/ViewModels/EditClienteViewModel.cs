using System.ComponentModel.DataAnnotations;

namespace HotelBookingApp2.ViewModels
{
    public class EditClienteViewModel
    {
        [Required]
        public Guid ClienteId { get; set; }

        [Required]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required]
        [Display(Name = "Cognome")]
        public string Cognome { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Telefono")]
        public string? Telefono { get; set; }
    }
}
