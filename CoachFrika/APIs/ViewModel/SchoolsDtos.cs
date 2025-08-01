using CoachFrika.Common.Enum;

namespace CoachFrika.APIs.ViewModel
{
    public class CreateSchoolDto
    {
        public string? SchoolName { get; set; }
        public string? SchoolAddress { get; set; }
        public int NumbersOfTeachers { get; set; }
        public string? Goals { get; set; }
        public string? ContactPersonEmail { get; set; }
        public string? ContactPersonName { get; set; }
        public string? ContactPersonPhoneNumber { get; set; }
    }
    public class CreateSchoolTeacherDto
    {
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public Guid? SchoolId { get; set; }
    }
}
