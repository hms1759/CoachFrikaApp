using CoachFrika.Extensions;
using CoachFrika.Models;
using CoachFrika.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Net.Http.Json;

namespace CoachFrika.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public readonly IConfiguration configuration;
        public readonly IEmailService _emailService;
        private readonly EmailConfigSettings _emailConfig;
        private HttpClient m_Client;
        public HomeController(ILogger<HomeController> logger, IEmailService emailService, IOptions<EmailConfigSettings> emailConfig)
        {
            _logger = logger;
            _emailConfig = emailConfig.Value;
            _emailService = emailService;
            m_Client = new HttpClient();
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ContactUs(ContactUs model)
        {
            if (!ModelState.IsValid)
            { //checking model state

                return RedirectToAction("Index");
            }
            // sending email
            var mailSubject = _emailConfig.ContactTopic;
            var mailto = _emailConfig.MailTo.ToList();
            var body = await _emailService.ReadTemplate("emailrecieved");
            var logoUrl = $"{Request.Scheme}://{Request.Host}/images/logo.png";

            //inserting variable
            var messageToParse = new Dictionary<string, string>
                    {
                        { "{Fullname}", model.FullName},
                        { "{Phonenumber}", model.PhoneNumber},
                        { "{Email}", model.Email},
                        { "{Message}", model.Message},
                        { "{logo}", logoUrl},
                    };

            //  email notification
            var messageBody = body.ParseTemplate(messageToParse);
            var message = new Message(mailto, mailSubject, messageBody);
            await _emailService.SendEmail(message);


            return RedirectToAction("Index");

        }
        [HttpPost]
        public async Task<IActionResult> RequestPlan(ContactUs model)
        {

            //HttpClient httpClient = new HttpClient();
            //string baseUrl = " https://sheetdb.io/api/v1/v0l3ssbj152qu";

            //HttpResponseMessage response =await  httpClient.PostAsJsonAsync(baseUrl,model);

            //if (response.IsSuccessStatusCode)
            //{
            //    string stateInfo = response.Content.ReadAsStringAsync().Result;
            //}
            // sending email
            var mailSubject = _emailConfig.RequestTopic;
            var body = await _emailService.ReadTemplate("planRequest");
            var logoUrl = $"{Request.Scheme}://{Request.Host}/images/logo.png";

            //inserting variable
            var messageToParse = new Dictionary<string, string>
                    {
                        { "{Fullname}", model.FullName},
                        { "{logo}", logoUrl},
                    };

            //  email notification
            var messageBody = body.ParseTemplate(messageToParse);
            var message = new Message(new List<string>{ model.Email }, mailSubject, messageBody);

            await _emailService.SendEmail(message);
       
            // sending email
            var Subject = _emailConfig.ContactTopic;
            var Recivedmail = _emailConfig.MailTo.ToList();
            var mailbody = await _emailService.ReadTemplate("requestNotification");

            //inserting variable
            var messagebodyToParse = new Dictionary<string, string>
                    {
                        { "{Fullname}", model.FullName},
                        { "{Phonenumber}", model.PhoneNumber},
                        { "{Email}", model.Email},
                        { "{School}", model.SchoolAddress},
                        { "{Address}", model.SchoolName},
                        { "{logo}", logoUrl},
                    };

            //  email notification
            var mailBody = mailbody.ParseTemplate(messagebodyToParse);
            var mailmessage = new Message(Recivedmail, Subject, mailBody);

           var check = await _emailService.SendEmail(mailmessage);

            if(check == null)
            {

            }
            return RedirectToAction("Index");

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}