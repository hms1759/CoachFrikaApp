using CoachFrika.APIs.ViewModel;
using CoachFrika.Common;
using CoachFrika.Models;
using coachfrikaaaa.APIs.Entity;

namespace CoachFrika.APIs.Domin.IServices
{
    public interface IStudentsService
    {
        Task<BaseResponse<string>> CreateStudents(CreateStudentsDto model);
        BaseResponse<List<Students>> GetAllStudents(GetStudentsSearch query);
        BaseResponse<ResponseScoreSheetDTo> GetAllStudentsScores(ScoreSheetsSearch query);
        Task<BaseResponse<string>> CreateStudentsScores(List<StudentScoreDto> model);
        //Task<BaseResponse<SchoolEnrollmentRequest>> GetStudentsById(Guid Id);
    }
}

