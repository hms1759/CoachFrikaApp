using CoachFrika.APIs.ViewModel;
using CoachFrika.Common;
using CoachFrika.Common.AutoMapper;

namespace CoachFrika.APIs.Domin.IServices
{
    public interface IAccountService
    {
        Task<BaseResponse<SignpStage1Resp>> SignUp(SignpUpDto signUpModel);
        Task<BaseResponse<LoginDetails>> Login(LoginDto login);
       Task<BaseResponse<string>> ForgetPassword(string email, string url);
       Task<BaseResponse<string>> ChangePassword(ChangePasswordDto model);
        Task<BaseResponse<string>> UploadFile(ProfileImgUpload model);
        Task<BaseResponse<string>> ResetPassword(ResetPasswordDto model);
        Task<BaseResponse<string>> GetProfileImageUrl();
        Task<BaseResponse<ProfileDto>> GetApplicant(string Id);
        Task<BaseResponse<string>> ApproveApplicantion(string Id);
        BaseResponse<ProfileDto> GetDetails(string Id);
    }
}
