using CoachFrika.APIs.ViewModel;

namespace CoachFrika.APIs.Domin.IServices
{
    public interface IPublicService
    {
        Task<PublicCountDto> GetPublicCount();
    }
}
