using coachfrikaaaa.Common;
using Microsoft.AspNetCore.Identity;

namespace coachfrikaaaa.APIs.Entity
{
    public class Schedule : BaseEntity
    {
        public string? Title { get; set; }
        public string? Focus { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? CoachId { get; set; }
        public CoachFrikaUsers? Coach { get; set; }
    }
}
