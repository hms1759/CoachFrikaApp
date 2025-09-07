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
        private readonly string _apiBaseUrl;
        public AccountController(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _apiBaseUrl = config["ApiBaseUrl"];
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
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid Model");
                return View(model);
            }
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync($"{_apiBaseUrl}/api/UserAccount/SignUp", model);

            if (!response.IsSuccessStatusCode)
            {
                var rawContent = await response.Content.ReadAsStringAsync();
                if (response.Content.Headers.ContentType?.MediaType == "application/json")
                {
                    // Parse JSON normally
                    //var errorContent = JsonSerializer.Deserialize<BaseResponse<SignpStage1Resp>>(rawContent);
                    ModelState.AddModelError(string.Empty, rawContent ?? "Unknown error");
                }
                else
                {
                    // Handle as plain text/HTML error
                    ModelState.AddModelError(string.Empty, "Server returned HTML error page: " + rawContent);
                }
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
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid Model");
                return View(model);
            }
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync($"{_apiBaseUrl}/api/UserAccount/Login", model);


            if (!response.IsSuccessStatusCode)
            {
                var rawContent = await response.Content.ReadAsStringAsync();
                if (response.Content.Headers.ContentType?.MediaType == "application/json")
                {
                    // Parse JSON normally
                    //var errorContent = JsonSerializer.Deserialize<BaseResponse<SignpStage1Resp>>(rawContent);
                    ModelState.AddModelError(string.Empty, rawContent ?? "Unknown error");
                }
                else
                {
                    // Handle as plain text/HTML error
                    ModelState.AddModelError(string.Empty, "Server returned HTML error page: " + rawContent);
                }
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

            HttpContext.Session.SetString("Token", JsonConvert.SerializeObject(token));
            HttpContext.Session.SetString("ProfileData", JsonConvert.SerializeObject(profile.Id));
            return RedirectToAction("Index", "Profile", profile);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            // Clear all session and temp data
            HttpContext.Session.Clear();
            TempData.Clear();

            // Optionally clear cookies if you're using any
            Response.Cookies.Delete(".AspNetCore.Session");

            return RedirectToAction("Login", "Account");
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
