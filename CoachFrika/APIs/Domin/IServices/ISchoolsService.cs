using CoachFrika.APIs.ViewModel;
using CoachFrika.Common;
using coachfrikaaaa.APIs.Entity;

namespace CoachFrika.APIs.Domin.IServices
{
    public interface ISchoolsService
    {
        Task<BaseResponse<string>> CreateSchools(CreateSchoolDto model);
        BaseResponse<List<SchoolEnrollmentRequest>> GetAllSchools(GetSchoolSearch query);
        Task<BaseResponse<SchoolEnrollmentRequest>> GetSchoolById(Guid Id);
        BaseResponse<List<CoachFrikaUsers>> GetSchoolTeachers(GetSchoolTeachersSearch query);
        Task<BaseResponse<string>> InviteSchoolTeacher(CreateSchoolTeacherDto model);
    }
}

