using CoachFrika.APIs.ViewModel;
using CoachFrika.Common;

namespace CoachFrika.APIs.Domin.IServices
{
    public interface ITeacherService
    {
        Task<BaseResponse<string>> CreateStage1(TitleDto model);
        Task<BaseResponse<string>> CreateStage2(TeacherPhoneYearsDto model);
        Task<BaseResponse<string>> CreateStage3(DescriptionDto model);
        Task<BaseResponse<string>> CreateStage4(SocialMediaDto model);
        Task<BaseResponse<string>> CreateStage5(SchoolesdescriptionDto model);
        Task<BaseResponse<string>> CreateStage6(SubscriptionsDto model);
        BaseResponse<List<ProfileDto>> MyTeachers(GetTeachers model);
        BaseResponse<List<SchedulesViewModel>> GetMySchedule(GetSchedules query);
        Task<BaseResponse<string>> SelectCoach(Guid CoachId);
        Task<BaseResponse<ProfileDto>> GetTeacherById(string Id);
    }
}

