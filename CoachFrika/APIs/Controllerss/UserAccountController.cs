using CoachFrika.APIs.Domin.IServices;
using CoachFrika.APIs.Domin.Services;
using CoachFrika.APIs.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoachFrika.APIs.Controllerss
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public UserAccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var result = await _accountService.Login(model);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] SignpUpDto model)
        {
            var result = await _accountService.SignUp(model);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] SubscriptionDto model)
        {
            var logoUrl = $"{Request.Scheme}://{Request.Host}/images/logo.png";
            var result = await _accountService.ForgetPassword(model.Email, logoUrl);
            return Ok(result);
        }
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var result = await _accountService.ChangePassword(model);
            return Ok(result);
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm]ProfileImgUpload file)
        {
            var result = await _accountService.UploadFile(file);
            return Ok(result);
           
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            var result = await _accountService.ResetPassword(model);
            return Ok(result);
        }
    }

}
