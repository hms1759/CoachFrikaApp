using CoachFrika.Common.Enum;
using coachfrikaaaa.Common;
using Microsoft.AspNetCore.Identity;

namespace coachfrikaaaa.APIs.Entity
{
    public class CoachFrikaUsers : IdentityUser
    {
        public string? FullName { get; set; }
        public string? TweeterUrl { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? FacebookUrl { get; set; }
        public bool IsDeleted { get; set; }
        public int Role { get; set; }
        public string? SecurityQuestion { get; set; }
        public string? SecurityAnswer { get; set; }
        public string? StateOfOrigin { get; set; }
        public string? ProfessionalTitle { get; set; }
        public string? Nationality { get; set; }
        public string? Address { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Subscriptions? Subscriptions { get; set; }
        public int NumbersOfStudents { get; set; }
        public DateTime? YearStartExperience { get; set; }
        public int Stages { get; set; }
        public string? CoachId { get; set; }
        public string? SchoolName { get; set; }
        public string? LocalGov { get; set; }
        public string? Subject { get; set; }
        public CoachFrikaUsers? Coach { get; set; }
        public Guid? TeacherId { get; set; }
        public Teachers? Teacher { get; set; }
    }
}
