using CoachFrika.Common.Enum;
using coachfrikaaaa.APIs.Entity;
using coachfrikaaaa.Common;
using Microsoft.AspNetCore.Identity;

namespace CoachFrika.APIs.ViewModel
{
    public class CreateScheduleDto
    {
        public string? Title { get; set; }
        public string? Focus { get; set; }
        public DateTime Scheduled { get; set; }
        public DurationType DurationType { get; set; }
        public int Duration { get; set; }
    }
    public class GetSchedules
    {
        public string? Title { get; set; }
        public DateTime Scheduled { get; set; }
        public ScheduleStatus status { get; set; }
        public int Pagesize { get; set; }
        public int PageNumber { get; set; }

    }
    public class SchedulesViewModel
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Focus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
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
    public class CreateScheduleaDto
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
