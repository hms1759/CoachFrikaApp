using CoachFrika.APIs.Domin.IServices;
using CoachFrika.APIs.Domin.Services;
using CoachFrika.APIs.ViewModel;
using CoachFrika.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoachFrika.APIs.Controllerss
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : BaseController
    {
        private readonly IAccountService _accountService;
        public UserAccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var result = await _accountService.Login(model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] SignpUpDto model)
        {
            var result = await _accountService.SignUp(model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [AllowAnonymous]
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] SubscriptionDto model)
        {
            var logoUrl = $"{Request.Scheme}://{Request.Host}/images/logo.png";
            var result = await _accountService.ForgetPassword(model.Email, logoUrl);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var result = await _accountService.ChangePassword(model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [Authorize]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm]ProfileImgUpload file)
        {  
            var result = await _accountService.UploadFile(file);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }

        [Authorize]
        [HttpGet("GetProfileImageUrl")]
        public async Task<IActionResult> GetProfileImageUrl()
        {
            var result = await _accountService.GetProfileImageUrl();
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            var result = await _accountService.ResetPassword(model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize]
        [HttpGet("userById")]
        public IActionResult UploadFile([FromQuery] Guid id)
        {
            var result =  _accountService.GetDetails(id.ToString());
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }
    }

}
