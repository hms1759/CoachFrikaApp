using CoachFrika.Common.Enum;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CoachFrika.APIs.ViewModel
{
    public class CreateSchoolDto
    {
        [Required]
        public string? SchoolName { get; set; }
        public string? SchoolAddress { get; set; }
        public int NumbersOfTeachers { get; set; }
        public string? Goals { get; set; }
        [Required]
        public string? ContactPersonEmail { get; set; }
        [Required]
        public string? ContactPersonName { get; set; }
        [Required]
        public string? ContactPersonPhoneNumber { get; set; }
    }
    public class CreateSchoolTeacherDto
    {
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        [JsonIgnore]
        public string? LogoUrl { get; set; }
        public Guid? SchoolId { get; set; }
    }
}
