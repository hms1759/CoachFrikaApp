using CoachFrika.APIs.ViewModel;
using CoachFrika.Common;
using coachfrikaaaa.APIs.Entity;

namespace CoachFrika.APIs.Domin.IServices
{
    public interface IStudentsService
    {
        Task<BaseResponse<string>> CreateStudents(CreateStudentsDto model);
        BaseResponse<List<SchoolEnrollmentRequest>> GetAllStudents(GetStudentsSearch query);
        Task<BaseResponse<SchoolEnrollmentRequest>> GetStudentsById(Guid Id);
    }
}

