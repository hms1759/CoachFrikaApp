using CoachFrika.APIs.ViewModel;
using CoachFrika.Common;
using CoachFrika.Models;
using coachfrikaaaa.APIs.Entity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
namespace CoachFrika.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserSignUpViewModel model)
        {

            var request = HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid Model");
                return View(model);
            }
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync($"{baseUrl}/api/UserAccount/SignUp", model);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadFromJsonAsync<BaseResponse<SignpStage1Resp>>();
                ModelState.AddModelError(string.Empty, errorContent.Message);

                return View(model);
            }

            var result = await response.Content.ReadFromJsonAsync<BaseResponse<SignpStage1Resp>>();
            if (result != null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error Occur");
                return View();
            }

        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {

            var request = HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid Model");
                return View(model);
            }
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync($"{baseUrl}/api/UserAccount/Login", model);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadFromJsonAsync<BaseResponse<LoginDetails>>();
                ModelState.AddModelError(string.Empty, errorContent.Message);

                return View(model);
            }

            var result = await response.Content.ReadFromJsonAsync<BaseResponse<LoginDetails>>();
            var resp = result.Data;
            var token = resp.Token;
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var claims = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);

            string profileContent = claims.ContainsKey("Profile") ? claims["Profile"] : null;
            var profile = JsonConvert.DeserializeObject<userProfileViewModel>(profileContent);
            var stage = profile.Stages;
            var partlySign = new SignpStage1Resp()
            {
                FullName = profile.FullName,
                Stage = stage,
                token = token,
            };
            if (profile.Role == 2)
            {
                return RedirectToAction( "Index", "BackOffice");

            }
            if (profile.Role == 1 && stage < 4)
            {
                return RedirectToAction("CoachModal", partlySign);
            }
            else if (profile.Role == 1 && stage == 4 && !profile.hasPaid)
            {
                return RedirectToAction("PostCoachRegistrationModal");
            }
            else if (profile.Role == 0 && stage < 6)
            {
                return RedirectToAction("Modal", partlySign);
            }
            else if (profile.Role == 0 && stage ==6 && !profile.hasPaid)
            {
                return RedirectToAction("PostPaymentModal");
            }
            TempData["Token"] = token;
            HttpContext.Session.SetString("ProfileData", JsonConvert.SerializeObject(profile));
            //TempData["ProfileData"] = JsonConvert.SerializeObject(profile);
            return RedirectToAction("Index", "Profile", profile);
        }

        [HttpGet]
        public IActionResult Modal(SignpStage1Resp partlySign)
        {
            return View(partlySign);
        }
       
        [HttpGet]
        public IActionResult PaymentModal(SignpStage1Resp partlySign)
        {
            return View(partlySign);
        }
        [HttpGet]
        public IActionResult PostPaymentModal()
        {
            return View();
        }
        [HttpGet]
        public IActionResult CoachModal(SignpStage1Resp partlySign)
        {
            return View(partlySign);
        }

        [HttpGet]
        public IActionResult PostCoachRegistrationModal()
        {
            return View();
        }
        //[HttpPost]
        //public IActionResult Modal(SignpStage1Resp partlySign)
        //{
        //    return View(partlySign);
        //}

    }
}
