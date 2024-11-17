using CoachFrika.APIs.ViewModel;
using CoachFrika.Common;
using coachfrikaaaa.APIs.Entity;

namespace CoachFrika.APIs.Domin.IServices
{
    public interface ICousesService
    {
        Task<BaseResponse<string>> CreateCourse(CreateCoursesDto model);
        Task<BaseResponse<string>> CreateBatches(BatchesDto model);
        BaseResponse<List<CoursesViewModel>> GetCourses();
        BaseResponse<List<BatchesViewModel>> GetBatches();
        Task<BaseResponse<string>> CreateSchedule(SchedulesDto model);
        BaseResponse<List<Schedules>> GetMySchedule();
    }
}
