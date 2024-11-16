using CoachFrika.APIs.ViewModel;
using coachfrikaaaa.APIs.Entity;
using Microsoft.EntityFrameworkCore;

namespace CoachFrika.APIs.Domin.IServices
{
    public interface ILogicService
    {
        Task<PublicCountDto> GetPublicCount();
        Task<string> NewSubscription(SubscriptionDto modle);
        Task<string> ContactUs(ContactUsDto modle);
        Task<string> CreateSchool(string modle);
        Task<string> CreateSubject(List<string> sub);
        Task<TeachersDTo> GetUserById(Guid modle);
        Task<TeachersDTo> GetUserDetails();
        string?[] GetSchool();
        string?[] GetSubject();
        List<Schedules> GetMySchedule();
       
    }
}
