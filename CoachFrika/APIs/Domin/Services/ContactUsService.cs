using CoachFrika.APIs.Domin.IServices;
using CoachFrika.APIs.ViewModel;
using CoachFrika.Common;
using CoachFrika.Common.AppUser;
using CoachFrika.Common.Enum;
using CoachFrika.Common.Extension;
using CoachFrika.Extensions;
using CoachFrika.Models;
using CoachFrika.Services;
using coachfrikaaaa.APIs.Entity;
using coachfrikaaaa.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Tls;
using System.Text;
using System.Text.RegularExpressions;
using static CoachFrika.Common.LogingHandler.JwtServiceHandler;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CoachFrika.APIs.Domin.Services
{
    public class ContactUsService : IContactUsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        private readonly EmailConfigSettings _emailConfig;
        public readonly IEmailService _emailService;
        public readonly IWebHelpers _webHelpers;
        public ContactUsService(IUnitOfWork unitOfWork, IOptions<EmailConfigSettings> emailConfig, AppDbContext context, IEmailService emailService, IWebHelpers webHelpers)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _emailConfig = emailConfig.Value;
            _emailService = emailService;
            _webHelpers = webHelpers;
        }

        public async Task<BaseResponse<string>> CreateContactUs(ContactFormModel model, string logoUrl)
        {
            var res = new BaseResponse<string>();
            res.Status = true;
            if(!IsValidPhoneNumber(model.PhoneNumber))
            {
                res.Status = false;
                res.Message = "invalid Phone number";
                return res;

            }
            if (IsValidEmail(model.Email) && await HasMxRecord(model.Email))
            {
                try
                {
                    var dto = new coachfrikaaaa.APIs.Entity.ContactUs();
                    dto.Email = model.Email;
                    dto.FullName = model.FullName;
                    dto.PhoneNumber = model.PhoneNumber;
                    dto.Message = model.Message;
                    var newsRepository = _unitOfWork.GetRepository<coachfrikaaaa.APIs.Entity.ContactUs>();
                    await newsRepository.AddAsync(dto);
                    await _unitOfWork.SaveChangesAsync();
                    var mailSubject = _emailConfig.ContactTopic;
                    var mailto = _emailConfig.MailTo.ToList();
                    var body = await _emailService.ReadTemplate("emailrecieved");

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
                    //await _emailService.SendEmail(message);

                    return res;
                }
                catch (Exception ex)
                {
                    res.Status = false;
                    res.Message = ex.Message;
                    return res;
                }
            }
            else
            {
                res.Status = false;
                res.Message = "invalid or unreachable email";
                return res;
            }
        }
        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public bool IsValidPhoneNumber(string phone)
        {
            return Regex.IsMatch(phone, @"^\+?[0-9]{7,15}$");
        }
        public async Task<bool> HasMxRecord(string email)
        {
            var domain = email.Split('@').LastOrDefault();
            if (string.IsNullOrEmpty(domain)) return false;

            var lookup = new DnsClient.LookupClient();
            var result = await lookup.QueryAsync(domain, DnsClient.QueryType.MX);
            return result.Answers.MxRecords().Any();
        }


    }
}
