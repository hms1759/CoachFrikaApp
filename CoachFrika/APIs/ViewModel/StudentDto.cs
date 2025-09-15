using CoachFrika.Common.Enum;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CoachFrika.APIs.ViewModel
{
    public class CreateStudentsDto
    {
        [Required]
        public ClassRoomEnum? ClassId { get; set; }
        [Required]
        public long? ClassNumber { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Address { get; set; }
        [Required]
        public string? ParentName { get; set; }
        [Required]
        public string? ParentPhoneNumber { get; set; }
        
    }

    public class GetStudentsSearch : Pagination
    {
        public string? Name { get; set; }
        public ClassRoomEnum? ClassId { get; set; }

    }
    public class ScoreSheetsSearch : Pagination
    {
        public SchoolSubjectEnum? SubjectId { get; set; }
        public string? StudentId { get; set; }
        public ClassRoomEnum? ClassId { get; set; }
        public bool isAll{ get; set; }

    }
}
