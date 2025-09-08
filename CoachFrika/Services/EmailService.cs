using CoachFrika.Controllers;
using CoachFrika.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Tls;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using Message = CoachFrika.Models.Message;

namespace CoachFrika.Services
{
    public class EmailMessage
    {
        public Sender Sender { get; set; }
        public List<Recipient> To { get; set; }
        public string Subject { get; set; }
        public string HtmlContent { get; set; }
    }

    public class Sender
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class Recipient
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }
    public class EmailService : IEmailService
    {
        private readonly EmailConfigSettings _emailConfig;
        private readonly ILogger<EmailService> _logger;
        private readonly IConfiguration _configuration;
        public EmailService(IOptions<EmailConfigSettings> emailConfig, ILogger<EmailService> logger, IConfiguration configuration)
        {
            _emailConfig = emailConfig.Value;
            _logger = logger;
            _configuration = configuration;
        }

        public Task<string> ReadTemplate(string messageType)
        {
            string htmlPath = Path.Combine(Environment.CurrentDirectory, @"wwwroot\html", "_template.html");
            string contentPath = Path.Combine(Environment.CurrentDirectory, @"wwwroot\html", $"{messageType}.txt");
            string html;
            string body;

            //get global html template
            if (File.Exists(htmlPath))
                html = File.ReadAllText(htmlPath);
            else
                return null;

            // get specific message content
            if (File.Exists(contentPath))
                body = File.ReadAllText(contentPath);
            else return null;

            string msgBody = html.Replace("{body}", body);
            return Task.FromResult(msgBody);
        }

        public async Task SendAsync(string to, string subject, string body, bool IsHTML, Attachment[] attachments)
        {
            var startTime = DateTime.Now;
            var stopWatch = Stopwatch.StartNew();

            var emailMessage = new EmailMessage
            {
                Sender = new Sender
                {
                    Name = _configuration["SMTP:CompanyName"],
                    Email = _configuration["SMTP:SendEmailFromMail"],
                },
                To = new List<Recipient>
            {
                new Recipient
                {

                    Email = to
                }
            },
                Subject = subject,
                HtmlContent = body
            };

            if (attachments is not null && attachments.Length > 0)
            {
                /**Add Attachments if any***/

                //   emailMessage.attachments.AddRange(attachments);

            }
            var request = JsonConvert.SerializeObject(emailMessage);
            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Add("api-key", _configuration["SMTP:APIKEY"]);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var payload = new
                    {
                        name = "Campaign sent via the API",
                        subject = "My subject",
                        sender = new { name = "Hr", email = "admin@rezumi.com" },
                        type = "classic",
                        htmlContent = "Congratulations! You successfully sent this example campaign via the Brevo API.",
                        recipients = new { listIds = new[] { "isola.topeyemi@gmail.com" } }
                    };
                    // Adjust the URL to the SendinBlue API endpoint
                    var apiUrl = _configuration["SMTP:url"];

                    var json = System.Text.Json.JsonSerializer.Serialize(payload);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Send request
                    var response = await client.PostAsync(apiUrl, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        // Handle error
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        _logger.LogError(JsonConvert.SerializeObject(errorResponse));
                        Console.WriteLine($"SendinBlue API error: {errorResponse}");
                        //               await _loggerAdapter.LogItem(level: Shared.Logger.LogLevel.Error, request, errorResponse, startTime,
                        //stopWatch.ElapsedMilliseconds, nameof(HttpMailClient.SendAsync), false);

                    }
                    else
                    {
                        //               await _loggerAdapter.LogItem(level: Shared.Logger.LogLevel.Custom, request, JsonConvert.SerializeObject(response), startTime,
                        //stopWatch.ElapsedMilliseconds, nameof(HttpMailClient.SendAsync), false);
                        Console.WriteLine("Email sent via SendinBlue.");
                    }
                }
                catch (Exception ex)
                {
                    //          await _loggerAdapter.LogItem(level: Shared.Logger.LogLevel.Error, request, null, startTime,
                    //stopWatch.ElapsedMilliseconds, nameof(HttpMailClient.SendAsync), false, JsonConvert.SerializeObject(ex));
                    _logger.LogError(JsonConvert.SerializeObject(ex));
                    Console.WriteLine($"Error sending email via SendinBlue: {ex.Message}");
                }


            }
        }
        public async Task<string> SendEmail(Message message)
        {
            var emailMessage = new MimeMessage();

            //sender
            emailMessage.From.Add(new MailboxAddress(_emailConfig.DisplayName, _emailConfig.From));

            //receiver
            foreach (string mailAddress in message.To)
                emailMessage.To.Add(MailboxAddress.Parse(mailAddress));

            //Add Content to Mime Message
            var content = new BodyBuilder();
            emailMessage.Subject = message.Subject;
            content.HtmlBody = message.Body;
            emailMessage.Body = content.ToMessageBody();
            try
            {
                //send email
                using var client = new MailKit.Net.Smtp.SmtpClient();

                await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);

                return "sent";
            }
            catch (Exception ex)
            {
                var error = ex.ToString();
                return null;
            }
        }
    }
}
