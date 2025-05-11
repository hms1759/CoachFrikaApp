using CoachFrika.APIs.ViewModel;
using CoachFrika.Common;
using CoachFrika.Models;

namespace CoachFrika.APIs.Domin.IServices
{
    public interface IContactUsService
    {
        Task<BaseResponse<string>> CreateContactUs(ContactFormModel model,string logoUrl);
    }
}

