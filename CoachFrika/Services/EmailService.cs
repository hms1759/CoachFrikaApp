using CoachFrika.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Mail;

namespace CoachFrika.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfigSettings _emailConfig;
        public EmailService(IOptions<EmailConfigSettings> emailConfig)
        {
            _emailConfig = emailConfig.Value;
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

            //send email
            using var client = new MailKit.Net.Smtp.SmtpClient();

                await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.None);
                await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                await client.SendAsync(emailMessage);

            return "sent";

        }
    }
}
