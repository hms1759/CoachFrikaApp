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

        public ActionResult Coaches()
        {
            return PartialView("_Coaches");
        }
        public ActionResult Teachers()
        {
            return PartialView("_Teachers");
        }
        public ActionResult Schedule()
        {
            return PartialView("_Schedule");
        }

        public ActionResult School()
        {
            return PartialView("_Schools");
        }

        [HttpGet("/BackOffice/GetApplicantDetails/{id}")]
        public async Task<ActionResult> GetApplicantDetails(string id)
        {
            var result = await _accountService.GetApplicant(id);
            return PartialView("_ApplicantDetails", result);
        }

    }
}
