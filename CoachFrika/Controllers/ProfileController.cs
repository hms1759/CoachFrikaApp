using CoachFrika.APIs.Domin.IServices;
using CoachFrika.APIs.ViewModel;
using CoachFrika.Common.Extension;
using CoachFrika.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CoachFrika.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ICoachesService _coachesService;
        public readonly IWebHelpers _webHelpers;

        public ProfileController(IAccountService accountService, ICoachesService coachesService, IWebHelpers webHelpers)
        {
            _accountService = accountService;
            _coachesService = coachesService;
            _webHelpers = webHelpers;
        }

        // GET: ProfileController
        public ActionResult Index(userProfileViewModel? model = null)
        {
            if (model == null || string.IsNullOrEmpty(model?.Email))
            {
                return RedirectToAction("Login", "Account");
            }
            if (model != null)
            {
                return View(model);
            }
            return View();
        }
        public IActionResult Dashboard()
        {
            var profileJson = HttpContext.Session.GetString("ProfileData");
            if (!string.IsNullOrEmpty(profileJson))
            {
                var model = JsonConvert.DeserializeObject<userProfileViewModel>(profileJson); ;
                if (model != null && !string.IsNullOrEmpty(model.Email))
                {
                    return PartialView("_Dashboard", model);
                }
            }

            return RedirectToAction("Index");
        }


        public IActionResult Schedule()
        {
            return PartialView("_Schedule");
        }

        public IActionResult MyCoach()
        {
            return PartialView("_MyCoach");
        }

        public IActionResult MyTeacher()
        {
            return PartialView("_MyTeacher");
        }


        public async Task<IActionResult> Details(string id)
        {
            var result = await _accountService.GetApplicant(id);
            return PartialView("_ApplicantDetails", result);
        }

        public IActionResult Recomendations(Guid id,Guid userId)
        {
            var request = new GetCoachesRecommendations
            {
                userId = userId.ToString(),
                TeacherId = id.ToString(),
                PageNumber = 1,
                Pagesize = 10
            };
            var recomendation = _coachesService.Recommendations(request);
            return PartialView("_Recomendations", recomendation);
        }
        [HttpPost]
        public IActionResult Setup()
        {
            return View();
        }
        //[HttpPost]
        //public IActionResult Setup(ProfileSetupViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    // Handle profile picture saving
        //    if (model.ProfilePicture != null && model.ProfilePicture.Length > 0)
        //    {
        //        var filePath = Path.Combine("wwwroot/uploads", Path.GetFileName(model.ProfilePicture.FileName));
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            model.ProfilePicture.CopyTo(stream);
        //        }
        //    }

        //    // Save other profile data to the database here

        //    return RedirectToAction("Dashboard");
        //}



    }
}
