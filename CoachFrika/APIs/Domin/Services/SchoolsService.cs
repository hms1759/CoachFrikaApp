using CloudinaryDotNet.Actions;
using CoachFrika.APIs.Domin.IServices;
using CoachFrika.APIs.Entity;
using CoachFrika.APIs.ViewModel;
using CoachFrika.Common;
using CoachFrika.Common.AppUser;
using CoachFrika.Common.AutoMapper;
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
using System.Text;
using System.Text.RegularExpressions;
using static CoachFrika.Common.LogingHandler.JwtServiceHandler;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CoachFrika.APIs.Domin.Services
{

    public class SchoolsService : ISchoolsService
    {
        private readonly AppDbContext _context;
        public readonly IEmailService _emailService;
        public readonly IWebHelpers _webHelpers;
        public readonly IAccountService _accountService;
        private readonly UserManager<CoachFrikaUsers> _userManager;
        private readonly UiSiteConfigSettings _uiSite;
        public SchoolsService(IUnitOfWork unitOfWork,
            AppDbContext context,
            IWebHelpers webHelpers, IOptions<UiSiteConfigSettings> uiSite,
            UserManager<CoachFrikaUsers> userManager, IAccountService accountService, IEmailService emailService)
        {
            _context = context;
            _webHelpers = webHelpers;
            _userManager = userManager;
            _uiSite = uiSite.Value;
            _accountService = accountService;
            _emailService = emailService;
        }

        public async Task<BaseResponse<string>> CreateSchools(CreateSchoolDto model)
        {
            var res = new BaseResponse<string>();
            res.Status = true;
            var sch = await _context.SchoolEnrollmentRequest.FirstOrDefaultAsync(x => x.ContactPersonEmail == model.ContactPersonEmail
            || x.ContactPersonPhoneNumber == model.ContactPersonPhoneNumber || x.SchoolName.ToLower().Contains(model.SchoolName.ToLower()));

            if (sch != null)
            {
                res.Message = "Schools already Onboarded";
                res.Status = false;
                return res;
            }

            var newEnt = new SchoolEnrollmentRequest()
            {
                SchoolName = model.SchoolName,
                SchoolAddress = model.SchoolAddress,
                SchoolPhoneNumber = model.ContactPersonPhoneNumber,
                SchoolEmail = model.ContactPersonEmail,
                NumbersOfTeachers = model.NumbersOfTeachers,
                Goals = model.Goals,
                ContactPersonEmail = model.ContactPersonEmail,
                ContactPersonName = model.ContactPersonName,
                ContactPersonPhoneNumber = model.ContactPersonPhoneNumber,
                isSubscribed = false
            };

            await _context.SchoolEnrollmentRequest.AddAsync(newEnt);
            _context.SaveChanges();
            res.Message = "Schools Successfully Onboarded";
            res.Status = true;
            return res;
        }

        public BaseResponse<List<SchoolEnrollmentRequest>> GetAllSchools(GetSchoolSearch query)
        {
            var userId = _webHelpers.CurrentUserId();
            var res = new BaseResponse<List<SchoolEnrollmentRequest>>();
            res.Status = true;
            try
            {
                // Apply filters based on the query parameters
                var cos = from rec in _context.SchoolEnrollmentRequest
                          where query.Name == null || rec.SchoolName.Contains(query.Name)
                          select rec;

                // Apply pagination using Skip and Take
                var pagedData = cos.Skip((query.PageNumber - 1) * query.Pagesize)
                                   .Take(query.Pagesize)
                                   .ToList();

                // Set the response data
                res.Data = pagedData;
                res.PageNumber = query.PageNumber;
                res.PageSize = query.Pagesize;
                res.TotalCount = cos.Count();
                return res;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
                return res;

            }

        }

        public async Task<BaseResponse<SchoolEnrollmentRequest>> GetSchoolById(Guid Id)
        {
            var res = new BaseResponse<SchoolEnrollmentRequest>();
            res.Status = true;
            try
            {
                var sch = await _context.SchoolEnrollmentRequest.FirstOrDefaultAsync(x => x.Id == Id);
                if (sch == null)
                {
                    res.Message = "School not found";
                    res.Status = false;
                    return res;
                }

                res.Data = sch;
                res.Status = false;
                return res;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
                return res;

            }

        }

        public BaseResponse<List<CoachFrikaUsers>> GetSchoolTeachers(GetSchoolTeachersSearch query)
        {
            var res = new BaseResponse<List<CoachFrikaUsers>>();
            res.Status = true;
            try
            {
                var day = DateTime.Now.Day;
                // Apply filters based on the query parameters
                var cos = _context.CoachFrikaUsers
                            .Where(t => t.Role == Roles.Teacher
                                && t.SchoolId.ToString() == query.SchoolId
                                && (string.IsNullOrEmpty(query.Name) || t.FullName.ToLower().Contains(query.Name.ToLower())))
                            .ToList();

                // Apply pagination using Skip and Take
                var pagedData = cos.Skip((query.PageNumber - 1) * query.Pagesize)
                                   .Take(query.Pagesize)
                                   .ToList();

                // Set the response data
                res.Data = pagedData;
                res.PageNumber = query.PageNumber;
                res.PageSize = query.Pagesize;
                return res;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
                return res;

            }
        }
        private async Task SendEmail(CreateSchoolTeacherDto user, string newPassword)
        {
            var subject = "Your Login Credentials ";
            var body = $"Your default password is: {newPassword} <br> click here <a href={_uiSite.SiteUrl}/login>here</a> to complete your registration ";

            var bodyTemplate = await _emailService.ReadTemplate("newPassword");
            //inserting variable
            var messageToParse = new Dictionary<string, string>
                    {
                        { "{Fullname}", user.Name},
                        { "{Message}", body},
                        { "{logo}", user.LogoUrl},
                    };

            //  email notification
            var messageBody = bodyTemplate.ParseTemplate(messageToParse);
            var message = new Message(new List<string> { user.Email }, subject, messageBody);

            await _emailService.SendEmail(message);
        }
        public async Task<BaseResponse<string>> InviteSchoolTeacher(CreateSchoolTeacherDto model)
        {
            var res = new BaseResponse<string>();
            res.Status = true;
            var sch = await _context.SchoolEnrollmentRequest.FirstOrDefaultAsync(x => x.Id == model.SchoolId);
            if (sch == null)
            {
                res.Message = "School not exist";
                res.Status = false;
                return res;
            }
            var teach = await _context.CoachFrikaUsers.FirstOrDefaultAsync(x => x.Email == model.Email);
            if (teach != null)
            {
                res.Message = "Teacher already exist";
                res.Status = false;
                return res;
            }
            var req = await _context.SchoolTeacherRequest.FirstOrDefaultAsync(x => x.Email == model.Email || x.PhoneNumber == model.PhoneNumber);
            if (req != null)
            {
                res.Message = "Teacher Request already exist";
                res.Status = false;
                return res;
            }
            var schTeacher = new CoachFrikaUsers()
            {
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                FullName = model.Name,
                Title = model.Title,
                hasPaid = true,
                SchoolId = model.SchoolId
            };

            var dd = new SchoolTeacherRequest
            {
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Title = model.Title,
                Name = model.Name
            };

            var newp = new SignpUpDto()
            {
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                FullName = model.Name,
                isCoach = false,
                SchoolId = model.SchoolId,
                Title = model.Title
                

            };
            try
            {
                newp.Password = GeneratePassword();
                var newUser = await _accountService.SignUp(newp);
                if (newUser == null )
                {
                    res.Status = false;
                    res.Message = "Ooops an error Occor";
                    return res;
                }

                if (newUser.Status == false)
                {
                    res.Status = false;
                    res.Message = newUser.Message;
                    return res;
                }

                _context.SchoolTeacherRequest.Add(dd);
                _context.SaveChanges();
                SendEmail(model, newp.Password);
                res.Status = true;
                res.Message = "Sucessfully requsted";
                return res;
            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = ex.Message;
                return res;

            }
        }
        public string GeneratePassword()
        {
            int length = 8;
            const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowercase = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string special = "@";

            var random = new Random();
            var password = new List<char>
    {
        uppercase[random.Next(uppercase.Length)],
        digits[random.Next(digits.Length)],
        special[random.Next(special.Length)],
        lowercase[random.Next(lowercase.Length)]
    };

            string allChars = uppercase + lowercase + digits + special;

            // Fill the rest of the password
            for (int i = password.Count; i < length; i++)
            {
                password.Add(allChars[random.Next(allChars.Length)]);
            }

            // Shuffle the password so required characters aren't predictable
            return new string(password.OrderBy(x => random.Next()).ToArray());
        }

    }
}
