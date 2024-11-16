using CoachFrika.APIs.ViewModel;

namespace CoachFrika.APIs.Domin.IServices
{
    public interface IAccountService
    {
        Task<SignpUpDto> SignUp(SignpUpDto signUpModel);
        Task<string> Login(LoginDto login);
        Task<string> ForgetPassword(string email, string url);
        Task<string> ChangePassword(ChangePasswordDto model);
    }
}
