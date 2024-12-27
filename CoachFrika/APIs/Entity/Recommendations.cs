using coachfrikaaaa.Common;
using Microsoft.AspNetCore.Identity;

namespace coachfrikaaaa.APIs.Entity
{
    public class Recommendations : BaseEntity
    {
        public string? Recommendation { get; set; }
        public string? CoachId { get; set; }
        public string? TeacherId { get; set; }
        public string? ScheduleId { get; set; }
        public Schedule? Schedule { get; set; }
        public string? TeacherRemark { get; set; }
    }
}
