using coachfrikaaaa.Common;
using Microsoft.AspNetCore.Identity;

namespace coachfrikaaaa.APIs.Entity
{
    public class Batches : BaseEntity
    {
        public string? Title { get; set; }
        public string? TeachersId { get; set; }
        public CoachFrikaUsers? Teachers { get; set; }
        public string? CourseId { get; set; }
        public Courses? Course { get; set; }
    }
}
