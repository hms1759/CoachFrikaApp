using CoachFrika.Common.Enum;
using coachfrikaaaa.APIs.Entity;
using System.Text.Json.Serialization;

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
    public class SchoolesdescriptionDto
    {
        public string? SchoolName { get; set; }
        public string? LocalGov { get; set; }
        public List<string> Subjects { get; set; }
    }

    public class SubscriptionsDto
    {
        [JsonIgnore]
        public string? Logo { get; set; }
        public Subscriptions Subscription { get; set; }
    }

    public class TeacherPhoneYearsDto : PhoneYearsDto
    {
        public int? NumberOfStudent { get; set; }
    }

    public class ProfileDto
    {
        public string? Id { get; set; }  
        public string? FullName { get; set; }
        public string? TweeterUrl { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? FacebookUrl { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? Email { get; set; }
        public int Role { get; set; }
        public string? StateOfOrigin { get; set; }
        public string? ProfessionalTitle { get; set; }
        public string? Nationality { get; set; }
        public string? Address { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Subscriptions? Subscriptions { get; set; }
        public int NumbersOfStudents { get; set; }
        public int? YearStartExperience { get; set; }
        public int Stages { get; set; }
        public string? CoachId { get; set; }
        public Guid? TeacherId { get; set; }
        public bool hasPaid { get; set; }
        public decimal Amount { get; set; }
        public bool IsPasswordDefault { get; set; }

    }
    public class GetTeachers : Pagination
    {
        public string? Name { get; set; }

    }
    public class GetAllCoaches : Pagination
    {
        public string? Name { get; set; }
        public bool IsPaginated { get; set; }

    }

    public class Pagination
    {
        public int Pagesize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;

    }

    public class CoachRecommendation
    {
        public Guid? ScheduleId { get; set; }
        public string? Recommendation { get; set; }

    }

    public class EditRecommendation
    {
        public Guid Id { get; set; }
        public string? Recommendation { get; set; }

    }
    public class TeachersRemark
    {
        public Guid Id { get; set; }
        public string? TeacherRemark { get; set; }

    }

    public class GetCoachesRecommendations : Pagination
    {
        public string? ScheduleTitle { get; set; }
        public string? TeachersName { get; set; }

    }


    public class GetTeacherRecommendations : Pagination
    {
        public string? ScheduleTitle { get; set; }

    }
    public class GetCoachesRecommendationResponse 
    {
        public string? Id { get; set; }
        public string? ScheduleTitle { get; set; }
        public string? Recommendation { get; set; }
        public string? TeachersName { get; set; }
        public string? CoachName { get; set; }
        public string? ScheduleId { get; set; }

    }
}
