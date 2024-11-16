using CoachFrika.APIs.Domin.IServices;
using CoachFrika.APIs.Domin.Services;
using CoachFrika.APIs.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoachFrika.APIs.Controllerss
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var result = await _accountService.Login(model);
            return Ok(result);
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] SignpUpDto model)
        {
            var result = await _accountService.SignUp(model);
            return Ok(result);
        }
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] string email)
        {
            var logoUrl = $"{Request.Scheme}://{Request.Host}/images/logo.png";
            var result = await _accountService.ForgetPassword(email, logoUrl);
            return Ok(result);
        }
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var result = await _accountService.ChangePassword(model);
            return Ok(result);
        }
    }
}
