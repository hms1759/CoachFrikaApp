using CoachFrika.Extensions;
using CoachFrika.Models;
using CoachFrika.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace CoachFrika.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public readonly IConfiguration configuration;
        public readonly IEmailService _emailService;
        private readonly EmailConfigSettings _emailConfig;
        public HomeController(ILogger<HomeController> logger, IEmailService emailService, IOptions<EmailConfigSettings> emailConfig)
        {
            _logger = logger;
            _emailConfig = emailConfig.Value;
            _emailService = emailService;
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
            var mailSubject = _emailConfig.MailTopic;
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}