using CoachFrika.APIs.ViewModel;
using CoachFrika.Common;
using CoachFrika.Common.Enum;
using coachfrikaaaa.APIs.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoachFrika.APIs.Domin.IServices
{
    public interface ILogicService
    {
        Task<BaseResponse<PublicCountDto>> GetPublicCount();
        Task<BaseResponse<string>> NewSubscription(SubscriptionDto modle);
        Task<BaseResponse<string>> ContactUs(ContactUsDto modle);
        Task<BaseResponse<string>> SchoolEnrollment(SchoolEnrollmentDto modle);
        Task<BaseResponse<string>> CreateSubject(List<SubjectDTO> sub);
        Task<BaseResponse<TeachersDTo>> GetUserById(Guid modle);
        Task<BaseResponse<TeachersDTo>> GetUserDetails();
        BaseResponse<string?[]> GetSchool();
        BaseResponse<List<Subjects>> GetSubject();
        BaseResponse<List<Schedule>> GetMySchedule();
        Task<BaseResponse<string>> SponsorAchild(SponsorDto modle);
        Task<BaseResponse<string>> CreateScheme(CreateSchemesDto model);
        BaseResponse<List<ResponseSchemesDto>> GetAllScheme(GetSchemeSearch query);
        BaseResponse<List<Schemes>> GetAllSchemeByPlan(Subscriptions? plan);
        Task<BaseResponse<string>> EditScheme(EditSchemesDto model);
        Task<BaseResponse<string>> DeleteScheme(Guid Id);
        Task<BaseResponse<List<UserTrendDto>>> GetUserTrend(TrendRequest trendRequest);
    }
}
