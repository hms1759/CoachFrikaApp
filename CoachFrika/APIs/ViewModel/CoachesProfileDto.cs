using CoachFrika.Common.Enum;
using coachfrikaaaa.APIs.Entity;
using Newtonsoft.Json;
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
        public List<string> Subjects { get; set; } = new List<string>();
    }

    public class SubscriptionsDto
    {
        [System.Text.Json.Serialization.JsonIgnore]
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
        public Roles Role { get; set; }
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
        public string? SchoolName { get; set; }
        public string? LocalGov { get; set; }
        public string? Subject { get; set; }
        public bool IsSelected { get; set; }
        public List<SchedulesViewModel>? Schedules { get; set; } = new List<SchedulesViewModel>();

    }
    
    public class GetRecomendationTeachers : Pagination
    {
        public string? Name { get; set; }
        public string? Recomendation { get; set; }

    }
    public class GetTeachers : Pagination
    {
        public bool IsPaginated { get; set; } = true;
        public string? Name { get; set; }

    }
    public class GetAllCoaches : Pagination
    {
        public string? Name { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public string? CoachId { get; set; }
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
        public List<string>? TeacherIds { get; set; }
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
        public string? TeacherId { get; set; }
        public Subscriptions? SelectedPlanType { get; set; }
        public int Pagesize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;

    }


    public class GetTeacherRecommendations : Pagination
    {
        public string? ScheduleTitle { get; set; }

    }
    public class GetSchoolSearch : Pagination
    {
        public string? Name { get; set; }

    }
    public class GetBackOfficeScheduleSearch : Pagination
    {
        public string? Name { get; set; }
        public Subscriptions? Plans { get; set; }

    }
    public class GetSchoolTeachersSearch : GetSchoolSearch
    {
        public string SchoolId { get; set; }
        public OnboardingStatus? OnboardingStatus { get; set; }

    }

    public class GetTeachersSearch : GetSchoolSearch
    {
        public bool IsCoach { get; set; }
        public OnboardingStatus? OnboardingStatus { get; set; }

    }
    public class GetTeacherRecommendationResponse
    {
        public string? Id { get; set; }
        public string? ScheduleTitle { get; set; }
        public string? Recommendation { get; set; }
        public string? TeachersName { get; set; }
        public string? CoachName { get; set; }
        public string? Remark { get; set; }
        public string? ScheduleId { get; set; }

    }
    public class GetCoachesRecommendationResponse
    {
        public string? Id { get; set; }
        public string? ScheduleTitle { get; set; }
        public string? ScheduleId { get; set; }
        public string? Recommendation { get; set; }
        public DateTime? CreatedDate { get; set; }
        public List<TeachersRemarks>? TeacherRemark { get; set; }

    }
    public class TeachersRemarks
    {
        public string? TeachersRemark{ get; set; }
        public string? TeachersName { get; set; }
    }
    public class CoachRecommendationComment
    {
        public long? Id { get; set; }
        public string? Comment { get; set; }

    }
    public class TrendRequest
    {
        public PeriodType PeriodType { get; set; }
        public DateTime? StartDate { get; set; } = DateTime.Now.AddDays(-1);
        public DateTime? EndDate { get; set; } = DateTime.Now;
    }
    public class UserTrendDto
    {
        public string Label { get; set; } = string.Empty;
        public int Teachers { get; set; }
        public int Coaches { get; set; }
    }
}
