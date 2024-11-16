using CoachFrika.APIs.ViewModel;
using coachfrikaaaa.APIs.Entity;

namespace CoachFrika.APIs.Domin.IServices
{
    public interface ICousesService
    {
        Task<string> CreateCourse(CreateCoursesDto model);
        Task<string> CreateBatches(BatchesDto model);
        List<CoursesViewModel> GetCourses();
        List<BatchesViewModel> GetBatches();
        Task<string> CreateSchedule(SchedulesDto model);
        List<Schedules> GetMySchedule();
    }
}
