using CoachFrika.Enums;
using System.ComponentModel.DataAnnotations;

namespace CoachFrika.Models
{
    public class ContactUs
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }

        //[StringLength(500, MinimumLength = 10, ErrorMessage = "Must be at least 10 characters long.")]

        public string? Message { get; set; }
        public string? SchoolName { get; set; }
        public string? SchoolAddress { get; set; }
        public Plans? Plan { get; set; }
        public string? YearsOfExperience { get; set; }
        public string? AreaofInterest { get; set; }
        public Referral Referral { get; set; }

        //[StringLength(500, MinimumLength = 10, ErrorMessage = "Must be at least 10 characters long.")]
        public string? WhyInterested { get; set; }

    }
}
