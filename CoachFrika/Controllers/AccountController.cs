using CoachFrika.Models;
using Microsoft.AspNetCore.Mvc;
namespace CoachFrika.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(UserSignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Save user to DB, hash password, etc.
                return RedirectToAction("Modal");
            }

            return View(model);
        }

        public IActionResult Login()
        {
            return View(); // Placeholder for login page
        }
        public IActionResult Modal()
        {
            return View(); // Placeholder for login page
        }
    }
}
