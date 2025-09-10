using CoachFrika.Common.Enum;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CoachFrika.APIs.ViewModel
{
    public class CreateStudentsDto
    {
        [Required]
        public long? ClassId { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Address { get; set; }
        [Required]
        public string? ParentName { get; set; }
        [Required]
        public string? Class { get; set; }
        [Required]
        public string? PhonePhoneNumber { get; set; }
    }

    public class GetStudentsSearch : Pagination
    {
        public string? Name { get; set; }

    }
}
