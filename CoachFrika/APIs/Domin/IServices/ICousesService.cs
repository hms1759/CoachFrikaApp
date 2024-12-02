using CoachFrika.APIs.ViewModel;
using CoachFrika.Common;
using coachfrikaaaa.APIs.Entity;

namespace CoachFrika.APIs.Domin.IServices
{
    public interface ICousesService
    {

        Task<BaseResponse<string>> CreateSchedule(CreateScheduleDto model);
        Task<BaseResponse<string>> EditSchedule(CreateScheduleDto model,Guid Id);
        BaseResponse<List<SchedulesViewModel>> GetCoachSchedule(GetSchedules query);
        //Task<BaseResponse<string>> CreateBatches(BatchesDto model);
        BaseResponse<List<CoursesViewModel>> GetCourses();
        //BaseResponse<List<BatchesViewModel>> GetBatches();
        BaseResponse<List<Schedules>> GetMySchedule();
    }
}
