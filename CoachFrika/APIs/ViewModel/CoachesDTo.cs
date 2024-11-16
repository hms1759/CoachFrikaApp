using coachfrikaaaa.APIs.Entity;
using coachfrikaaaa.Common;
using Microsoft.AspNetCore.Identity;

namespace CoachFrika.APIs.ViewModel
{
    public class CreateCoursesDto
    {
        public string? CourseTitle { get; set; }
        public string? CourseIntro { get; set; }
    }
    public class BatchesDto
    {
        public string? Title { get; set; }
    }
    public class CoursesViewModel
    {
        public Guid Id { get; set; }
        public string? CourseTitle { get; set; }
    }
    public class BatchesViewModel
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
    }
    public class CreateScheduleDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
    }
    public class SchedulesDto
    {
        public string? Description { get; set; }
        public string? Subject { get; set; }
        public string? MaterialUrl { get; set; }
        public DateTime? Schedule { get; set; }
        public Guid BatcheId { get; set; }
        public Guid CourseId { get; set; }
    }
}
