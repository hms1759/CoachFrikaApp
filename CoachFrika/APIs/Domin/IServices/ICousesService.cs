using CoachFrika.APIs.ViewModel;
using CoachFrika.Common;
using coachfrikaaaa.APIs.Entity;

namespace CoachFrika.APIs.Domin.IServices
{
    public interface ICousesService
    {

        Task<BaseResponse<string>> CreateSchedule(CreateScheduleDto model);
        BaseResponse<string> EditSchedule(EditScheduleDto model);
        BaseResponse<List<SchedulesViewModel>> GetCoachSchedule(GetSchedules query);
        BaseResponse<List<SchedulesViewModel>> GetScheduleList();
        //Task<BaseResponse<string>> CreateBatches(BatchesDto model);
        BaseResponse<List<CoursesViewModel>> GetCourses();
        //BaseResponse<List<BatchesViewModel>> GetBatches();
        BaseResponse<List<Schedule>> GetMySchedule();
        BaseResponse<string> AttendSchedle(Guid Id);
        BaseResponse<SchedulesDto> GetScheduleById(Guid Id);
        BaseResponse<List<ProfileDto>> GetTeacherList(string ScheduleId);
    }
}
