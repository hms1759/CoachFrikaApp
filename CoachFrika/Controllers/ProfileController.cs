using CoachFrika.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoachFrika.Controllers
{
    public class ProfileController : Controller
    {
        // GET: ProfileController
        public ActionResult Index()
        {
            if (Request.Headers["X-Requested-With"] != "XMLHttpRequest")
            {
                return RedirectToAction("SignUp","Account");
            }
            return View();
        }
        public IActionResult Dashboard(userProfileViewModel? model)
        {
            return PartialView("_Dashboard");
        }

        public IActionResult Schedule()
        {
            return PartialView("_Schedule");
        }

        public IActionResult MyCoach()
        {
            return PartialView("_MyCoach");
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
