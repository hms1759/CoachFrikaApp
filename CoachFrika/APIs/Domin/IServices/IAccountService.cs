using CoachFrika.APIs.ViewModel;
using CoachFrika.Common;

namespace CoachFrika.APIs.Domin.IServices
{
    public interface IAccountService
    {
        Task<BaseResponse<SignpUpDto>> SignUp(SignpUpDto signUpModel);
        Task<BaseResponse<LoginDetails>> Login(LoginDto login);
       Task<BaseResponse<string>> ForgetPassword(string email, string url);
       Task<BaseResponse<string>> ChangePassword(ChangePasswordDto model);
        bool ValidatePhoneNumber(string phoneNumber);
    }
}
