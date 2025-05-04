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

        public IActionResult Report()
        {
            return PartialView("_Report");
        }

        public IActionResult Settings()
        {
            return PartialView("_Settings");
        }
    }
}
