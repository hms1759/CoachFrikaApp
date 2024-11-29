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
    }
}
