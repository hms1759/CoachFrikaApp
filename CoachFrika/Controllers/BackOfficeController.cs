using CoachFrika.APIs.Domin.IServices;
using CoachFrika.APIs.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

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
        [HttpPost("/BackOffice/whatsap")]
        public async Task<IActionResult> whatsapp([FromBody]string mssg)
        {
            var accountSid = "AC841a7246326c08743a53912148a3e275";
            var authToken = "HX350d429d32e64a552466cafecbe95f3c";
            TwilioClient.Init(accountSid, authToken);

            var messageOptions = new CreateMessageOptions(
              new PhoneNumber("whatsapp:+2348068783985"));
            messageOptions.From = new PhoneNumber("whatsapp:+14155238886");
            //messageOptions.contentSid = "HX350d429d32e64a552466cafecbe95f3c";
            //messageOptions.variables = "{"1":"12 / 1","2":"3pm"}";

            messageOptions.Body = mssg;


            var message = MessageResource.Create(messageOptions);
            Console.WriteLine(message.Body);
            return null;
        }
    }
}
