using CoachFrika.Extensions;
using CoachFrika.GoogleExtension;
using CoachFrika.Models;
using CoachFrika.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Net.Http.Json;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource;

namespace CoachFrika.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public readonly IConfiguration configuration;
        public readonly IEmailService _emailService;
        private readonly EmailConfigSettings _emailConfig;
        private HttpClient m_Client; 
        const string SPREADSHEET_ID = "1RrG88SEXb5p85PzQUXhfzccHDa97hdAXyeueoqnUbSk";
        const string SHEET_NAME = "Sheets";
        SpreadsheetsResource.ValuesResource _googleSheetValues;

        public HomeController(ILogger<HomeController> logger, GoogleSheetsHelper googleSheetsHelper, IEmailService emailService, IOptions<EmailConfigSettings> emailConfig)
        {
            _logger = logger;
            _emailConfig = emailConfig.Value;
            _emailService = emailService;
            m_Client = new HttpClient();
            _googleSheetValues = googleSheetsHelper.Service.Spreadsheets.Values;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ContactUs(ContactUs model)
        {
            //if (!ModelState.IsValid)
            //{ //checking model state

            //    return RedirectToAction("Index");
            //}
            //var range = "Sheet2!A1:D5";
            //var valueRange = new ValueRange
            //{
            //    Values = GoogleMapper.MapToRangeContactData(model)
            //};

            //var appendRequest = _googleSheetValues.Append(valueRange, SPREADSHEET_ID, range);
            //appendRequest.ValueInputOption = AppendRequest.ValueInputOptionEnum.USERENTERED;
            //appendRequest.Execute();
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
            var range = "Sheet1!A1:D5";
            var valueRange = new ValueRange
            {
                Values = GoogleMapper.MapToRangeData(model)
            };

            var appendRequest = _googleSheetValues.Append(valueRange, SPREADSHEET_ID, range);
            appendRequest.ValueInputOption = AppendRequest.ValueInputOptionEnum.USERENTERED;
            appendRequest.Execute();

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