using CoachFrika.APIs.ViewModel;

namespace CoachFrika.APIs.Domin.IServices
{
    public interface IAccountService
    {
        Task<SignpUpDto> SignUp(SignpUpDto signUpModel);
        Task<string> Login(LoginDto login);
    }
}
