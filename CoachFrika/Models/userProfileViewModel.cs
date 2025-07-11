using CoachFrika.APIs.ViewModel;
using CoachFrika.Common.Enum;
using coachfrikaaaa.APIs.Entity;

namespace CoachFrika.Models
{
    public class userProfileViewModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string TweeterUrl { get; set; }
        public string LinkedInUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
        public string StateOfOrigin { get; set; }
        public string ProfessionalTitle { get; set; }
        public string Nationality { get; set; }
        public string Address { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Subscriptions? Subscriptions { get; set; } 
        public int NumbersOfStudents { get; set; }
        //public int YearStartExperience { get; set; }
        public int Stages { get; set; }
        public Guid? CoachId { get; set; }
        public Guid? TeacherId { get; set; }
        public bool hasPaid { get; set; }
        public decimal Amount { get; set; }
        public string SchoolName { get; set; }
        public string LocalGov { get; set; }
        public string Subject { get; set; }
        public List<SchedulesViewModel>? Schedules { get; set; } = new List<SchedulesViewModel>();
    }

}
