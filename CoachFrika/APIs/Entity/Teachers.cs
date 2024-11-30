using coachfrikaaaa.Common;
using Microsoft.AspNetCore.Identity;

namespace coachfrikaaaa.APIs.Entity
{
    public class Teachers : BaseEntity
    {
        public string? School { get; set; }
        public int NumbersOfStudents { get; set; }
        public int YearOfExperience { get; set; }
    }
}
