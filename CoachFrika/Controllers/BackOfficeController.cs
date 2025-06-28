using CoachFrika.APIs.Domin.IServices;
using CoachFrika.APIs.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoachFrika.Controllers
{
    public class BackOfficeController : Controller
    {
        private readonly ITeacherService _teacherService;
        private readonly ICoachesService _coachesService;
        private readonly IAccountService _accountService;
        public BackOfficeController(ITeacherService teacherService, ICoachesService coachesService, IAccountService accountService)
        {
            _teacherService = teacherService;
            _coachesService = coachesService;
            _accountService = accountService;
        }
        // GET: BackOfficeController1cs
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return PartialView("_Dashboard");
        }

        public ActionResult Coaches([FromQuery] GetTeachersSearch model)
        {
            var result = _coachesService.GetAllCoaches(model);
            
            return PartialView("_Coaches", result);
        }
        public ActionResult Teachers([FromQuery] GetTeachersSearch model)
        {
            var result = _teacherService.GetTeachers(model);
            return PartialView("_Teachers", result);
        }
        public ActionResult Schedule()
        {
            return PartialView("_Schedule");
        }

        // GET: BackOfficeController1cs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BackOfficeController1cs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BackOfficeController1cs/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BackOfficeController1cs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BackOfficeController1cs/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BackOfficeController1cs/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
