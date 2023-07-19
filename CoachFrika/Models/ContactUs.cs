using System.ComponentModel.DataAnnotations;

namespace CoachFrika.Models
{
    public class ContactUs
    {
        [Required(ErrorMessage = "Please provide your full name ")]
        public string? FullName { get; set; }
        [Required (ErrorMessage ="Phone number is required")]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "Email  is required")]
        public string? Email { get; set; }

        [StringLength(500, MinimumLength = 10, ErrorMessage = "Must be at least 10 characters long.")]

        public string? Message { get; set; }
    }
}
