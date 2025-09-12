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
        BaseResponse<List<ScoreSheetDTo>> GetAllStudentsScores(ScoreSheetsSearch query);
        //Task<BaseResponse<SchoolEnrollmentRequest>> GetStudentsById(Guid Id);
    }
}

