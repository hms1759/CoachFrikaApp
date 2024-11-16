using coachfrikaaaa.Common;
using Microsoft.AspNetCore.Identity;

namespace coachfrikaaaa.APIs.Entity
{
    public class Courses : BaseEntity
    {
        public string? CourseTitle { get; set; }
        public string? CourseIntro { get; set; }
        public string? CoachId { get; set; }
        public CoachFrikaUsers? Coach { get; set; }
    }
}
