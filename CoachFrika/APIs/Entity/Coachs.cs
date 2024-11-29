using coachfrikaaaa.Common;
using Microsoft.AspNetCore.Identity;

namespace coachfrikaaaa.APIs.Entity
{
    public class Coaches : BaseEntity
    {
        public string? ProfessionalTitle { get; set; }
        public Guid CoachFrikaUserId { get; set; }
        public CoachFrikaUsers? CoachFrikaUser { get; set; }
    }
}
