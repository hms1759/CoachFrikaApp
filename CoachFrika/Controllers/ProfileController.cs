using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoachFrika.Controllers
{
    public class ProfileController : Controller
    {
        // GET: ProfileController
        public ActionResult Index()
        {
            return View();
        }
        public IActionResult Dashboard()
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
    }
}
