using CoachFrika.APIs.ViewModel;
using CoachFrika.Common;
using coachfrikaaaa.APIs.Entity;

namespace CoachFrika.APIs.Domin.IServices
{
    public interface ICoachesService
    {
        Task<BaseResponse<string>> CreateStage1(TitleDto model);
        Task<BaseResponse<string>> CreateStage2(PhoneYearsDto model);
        Task<BaseResponse<string>> CreateStage3(DescriptionDto model);
        Task<BaseResponse<string>> CreateStage4(SocialMediaDto model);
        Task<BaseResponse<string>> CreateStage5(SubscriptionsDto model);
        BaseResponse<List<ProfileDto>> MyTeachers(GetTeachers model);
        BaseResponse<List<ProfileDto>> GetAllCoaches(GetAllCoaches query);
        Task<BaseResponse<ProfileDto>> GetCoachById(string Id);
        Task<BaseResponse<string>> AddRecomendations(CoachRecommendation model);
        Task<BaseResponse<string>> EditRecommendation(EditRecommendation model);
        BaseResponse<List<GetCoachesRecommendationResponse>> Recommendations(GetCoachesRecommendations query);
        Task<BaseResponse<Recommendations>> GetRecommendationById(string Id);
    }
}

