using coachfrikaaaa.Common;

namespace CoachFrika.APIs.Entity
{
    public class SchoolTeacherRequest : BaseEntity
    {
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
