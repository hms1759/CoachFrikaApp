using CoachFrika.APIs.ViewModel;
using CoachFrika.Common;
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
        private readonly IHttpClientFactory _httpClientFactory;


        public HomeController(ILogger<HomeController> logger,
            IEmailService emailService, IOptions<EmailConfigSettings> emailConfig,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _emailConfig = emailConfig.Value;
            _emailService = emailService;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            try { 

            var request = HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetFromJsonAsync<BaseResponse<PublicCountDto>>($"{baseUrl}/api/Public/GetLandingPageCount");
            if(response.Data == null)
            {
                return View();

            }
            return View(response?.Data);
            }
            catch
            {
                return View();

            }
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Pricing()
        {
            return View();
        }
        public IActionResult Service()
        {
            return View();
        }
        public IActionResult Blog()
        {
            return View();
        }
        //public IActionResult Dashboard()
        //{
        //    return View();
        //}

        public IActionResult JoinUs()
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}