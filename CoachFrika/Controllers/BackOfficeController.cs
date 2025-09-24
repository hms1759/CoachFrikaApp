using CoachFrika.APIs.Domin.IServices;
using CoachFrika.APIs.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoachFrika.Controllers
{
    public class BackOfficeController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ISchoolsService _schoolService;
        public BackOfficeController(IAccountService accountService, ISchoolsService schoolService)
        {
            _accountService = accountService;
            _schoolService = schoolService;
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
        public ActionResult Scheme()
        {
            return PartialView("_Scheme");
        }
        
        public ActionResult Schools()
        {
            return PartialView("_Schools");
        }

        [HttpGet("/BackOffice/GetApplicantDetails/{id}")]
        public async Task<ActionResult> GetApplicantDetails(string id)
        {
            var result = await _accountService.GetApplicant(id);
            return PartialView("_ApplicantDetails", result);
        }

        //[HttpPut("/BackOffice/SchoolTeacher/{Id}")]
        public async Task<IActionResult> SchoolTeacher(Guid Id)
        {
            var ss = new GetSchoolTeachersSearch
            {
                SchoolId = Id.ToString(),
            };
            return PartialView("_SchoolTeachers", Id);
        }
       
    }
}
