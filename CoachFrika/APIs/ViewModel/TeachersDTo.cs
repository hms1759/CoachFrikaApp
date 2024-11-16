using coachfrikaaaa.APIs.Entity;
using Microsoft.AspNetCore.Identity;

namespace CoachFrika.APIs.ViewModel
{
    public class TeachersDTo 
    {
        public string? Id { get; set; }
        public string? FullName { get; set; }
        public string? TweeterUrl { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? FacebookUrl { get; set; }
        public string? Address { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int NumbersOfStudents { get; set; }
        public int YearOfExperience { get; set; }
        public string? School { get; set; }
        public string[]? Subjects { get; set; }
    }
}
