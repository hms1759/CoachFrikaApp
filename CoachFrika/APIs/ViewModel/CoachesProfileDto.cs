using CoachFrika.Common.Enum;
using coachfrikaaaa.APIs.Entity;

namespace CoachFrika.APIs.ViewModel
{
    public class SocialMediaDto
    {
        public string? TweeterUrl { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? FacebookUrl { get; set; }
    }
    public class TitleDto
    {
        public string? ProfessionalTitle { get; set; }
        public string? Title { get; set; }
    }
    public class PhoneYearsDto
    {
        public string? PhoneNumber { get; set; }
        public int YearOfExperience { get; set; }
    }
    public class DescriptionDto
    {
        public string? StateOfOrigin { get; set; }
        public string? Nationality { get; set; }
        public string? Description { get; set; }
    }

    public class SubscriptionsDto
    {
        public Subscriptions Subscription { get; set; }
    }
    public class ProfileDto {

        public string? FullName { get; set; }
        public string? TweeterUrl { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? FacebookUrl { get; set; }
        public int Role { get; set; }
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
        public Guid? CoachId { get; set; }
        public Coaches? Coach { get; set; }
        public Guid? TeacherId { get; set; }
        public Teachers? Teacher { get; set; }
        public Guid? SchoolId { get; set; }
        public Schools? School { get; set; }
    }
    }
