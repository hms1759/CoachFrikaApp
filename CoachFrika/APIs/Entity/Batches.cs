using coachfrikaaaa.Common;
using Microsoft.AspNetCore.Identity;

namespace coachfrikaaaa.APIs.Entity
{
    public class Batches : BaseEntity
    {
        public string? Title { get; set; }
        public Guid? TeachersId { get; set; }
        public CoachFrikaUsers? Teachers { get; set; }
        public Guid? CourseId { get; set; }
        public Courses? Course { get; set; }
    }
}
