using CoachFrika.APIs.Domin.IServices;
using CoachFrika.APIs.Entity;
using CoachFrika.APIs.ViewModel;
using CoachFrika.Common;
using CoachFrika.Common.AppUser;
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

namespace CoachFrika.APIs.Domin.Services
{
    public class LogicService : ILogicService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        private readonly EmailConfigSettings _emailConfig;
        public readonly IEmailService _emailService;
        public readonly IWebHelpers _webHelpers;
        public LogicService(IUnitOfWork unitOfWork, IOptions<EmailConfigSettings> emailConfig, AppDbContext context, IEmailService emailService, IWebHelpers webHelpers)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _emailConfig = emailConfig.Value;
            _emailService = emailService;
            _webHelpers = webHelpers;
        }
        
        public async Task<BaseResponse<string>> SchoolEnrollment(SchoolEnrollmentDto model)
        {
            var res = new BaseResponse<string>();
            res.Status = true;
            try
            {
                var dto = new coachfrikaaaa.APIs.Entity.SchoolEnrollmentRequest();
                
                var phoneNumberValid = Validators.ValidatePhoneNumber(model.ContactPersonPhoneNumber);
                if (!phoneNumberValid)
                {
                    res.Message = "Phone number must be in the format: 0800 000 0000";
                    res.Status = false;
                    return res;
                }
                // Validate email format
                var emailValid = Validators.ValidateEmail(model.ContactPersonEmail);
                if (!emailValid)
                {
                    res.Message = "Invalid email format.";
                    res.Status = false;
                    return res;
                }
                dto.ContactPersonEmail = model.ContactPersonEmail;
                dto.ContactPersonPhoneNumber = model.ContactPersonPhoneNumber;

                dto.SchoolName = model.SchoolName;
                dto.SchoolAddress = model.SchoolAddress;
                var newsRepository = _unitOfWork.GetRepository<coachfrikaaaa.APIs.Entity.SchoolEnrollmentRequest>();
                await newsRepository.AddAsync(dto);
                await _unitOfWork.SaveChangesAsync();
                var mailSubject = _emailConfig.ContactTopic;
                var mailto = _emailConfig.MailTo.ToList();
                var body = await _emailService.ReadTemplate("emailrecieved");

                //inserting variable
                var messageToParse = new Dictionary<string, string>
                    {
                        { "{Fullname}", model.ContactPersonName},
                        { "{Phonenumber}", model.ContactPersonPhoneNumber},
                        { "{Email}", model.ContactPersonEmail},
                        { "{Message}", $"The {model.SchoolName} has request for enrollment"},
                        { "{logo}", model.logoUrl},
                    };

                //  email notification
                var messageBody = body.ParseTemplate(messageToParse);
                var message = new Message(mailto, mailSubject, messageBody);
                //await _emailService.SendEmail(message);

                return res;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
                return res;

            }
        }

        public async Task<BaseResponse<string>> ContactUs(ContactUsDto model)
        {
            var res = new BaseResponse<string>();
            res.Status = true;
            try
            {

                var phoneNumberValid = Validators.ValidatePhoneNumber(model.PhoneNumber);
                if (!phoneNumberValid)
                {
                    res.Message = "Phone number must be in the format: 0800 000 0000";
                    res.Status = false;
                    return res;
                }
                // Validate email format
                var emailValid = Validators.ValidateEmail(model.Email);
                if (!emailValid)
                {
                    res.Message = "Invalid email format.";
                    res.Status = false;
                    return res;
                }
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
                        { "{logo}", model.logoUrl},
                    };

                //  email notification
                var messageBody = body.ParseTemplate(messageToParse);
                var message = new Message(mailto, mailSubject, messageBody);
                //await _emailService.SendEmail(message);

                return res;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
                return res;

            }
        }

        public async Task<BaseResponse<PublicCountDto>> GetPublicCount()
        {

            var res = new BaseResponse<PublicCountDto>();
            res.Status = true;
            try
            {
                var schcount = await _context.SchoolEnrollmentRequest.Where(x=> x.isSubscribed).ToListAsync();
                var usercount = await _context.CoachFrikaUsers.Where(x => x.Role == 1 || x.Role == 0).ToListAsync();
                //if (usercount.Count() >1)
                //    res.Message = "User not found");
                var dto = new PublicCountDto();
                // Get count of users with Coach role
                dto.CoachesCount = usercount.Where(x => x.Role == 1).Count();
                // Get count of users with Teacher role
                var Teachers = usercount.Where(x => x.Role == 0).ToList();
                dto.TeachersCount = Teachers.Count;
                dto.StudentCount = Teachers.Sum(x => x.NumbersOfStudents);
                dto.SchoolCount = schcount.Count;
                res.Data = dto;
                return res;
            }

            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
                return res;

            }
        }

        public async Task<BaseResponse<string>> NewSubscription(SubscriptionDto model)
        {

            var res = new BaseResponse<string>();
            res.Status = true;
            try
            { // Validate email format
                var emailValid = Validators.ValidateEmail(model.Email);
                if (!emailValid)
                {
                    res.Message = "Invalid email format.";
                    res.Status = false;
                    return res;
                }
                var dto = new NewsSubscription();
                dto.Email = model.Email;
                var newsRepository = _unitOfWork.GetRepository<NewsSubscription>();
                await newsRepository.AddAsync(dto);
                await _unitOfWork.SaveChangesAsync();
                res.Message = "Successful";
                return res;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
                return res;

            }
        }
   public async Task<BaseResponse<string>> CreateSubject(List<string> sub)
        {

            var res = new BaseResponse<string>();
            res.Status = true;
            try
            {
                var subRepository = _unitOfWork.GetRepository<Subjects>();
                var dtoList = new List<Subjects>();
                foreach (var subItem in sub)
                {

                    var dto = new Subjects();
                    dto.SubjectName = subItem;
                    dtoList.Add(dto);
                }
                await subRepository.AddRangeAsync(dtoList);
                await _unitOfWork.SaveChangesAsync();
                res.Message = "Successful";
                return res;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
                return res;

            }
        }

        public async Task<BaseResponse<TeachersDTo>> GetUserById(Guid Id)
        {

            var res = new BaseResponse<TeachersDTo>();
            res.Status = true;
            var userRepo = _unitOfWork.GetRepository<CoachFrikaUsers>();
            var user = await userRepo.GetByIdAsync(Id);
            var rr = await GetUserByEmail(user?.Email);
             res.Data = rr;
            return res;
        }

        public async Task<BaseResponse<TeachersDTo>> GetUserDetails()
        {

            var res = new BaseResponse<TeachersDTo>();
            res.Status = true;
            var email = _webHelpers.CurrentUser();
            var rr = await GetUserByEmail(email);
            res.Data = rr;
            return res;
        }
        public async Task<TeachersDTo> GetUserByEmail(string email)
        {
            var year = DateTime.Now.Year;
            try
            {

                var role = _webHelpers.CurrentUserRole();
                var user = from users in _context.CoachFrikaUsers
                           //join sch in _context.Schools on users.SchoolId equals sch.Id
                           where users.Email.ToLower() == email.Trim().ToLower()
                           select new TeachersDTo
                           {
                               Id = users.Id,
                               FullName = users.FullName,
                               TweeterUrl = users.TweeterUrl,
                               LinkedInUrl = users.LinkedInUrl,
                               InstagramUrl = users.InstagramUrl,
                               FacebookUrl = users.FacebookUrl,
                               Address = users.Address,
                               Title = users.Title,
                               Description = users.Description,
                               NumbersOfStudents = users.NumbersOfStudents,
                               //YearOfExperience = year - users.YearStartExperience.Year,
                               //School = sch.School,
                                  };

                var teacherDto = await user.FirstOrDefaultAsync();

                if (role.Contains(AppRoles.Coach))
                {
                    var Id = _webHelpers.CurrentUserId();
                    //var teacher = from Courses in _context.Schedule
                    //              join bat in _context.Batches on Courses.Id equals bat.CourseId
                    //              join teach in _context.CoachFrikaUsers on bat.TeachersId.ToString() equals teach.Id
                    //              where Courses.CoachId == Id
                    //              select new TeachersDTo
                    //           {
                    //               Id = teach.Id,
                    //               FullName = teach.FullName
                    //           };
                    //teacherDto.NumbersOfStudents =  teacher.ToList().Count();
                }
                return teacherDto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public BaseResponse<string?[]> GetSchool()
        {
            var res = new BaseResponse<string?[]>();
            res.Status = true;
            var schs = from sch in _context.SchoolEnrollmentRequest
                       select sch;
           // var scharray = schs.Select(x => x.School).ToArray();
            res.Data = null;
            return res;
        }

        public BaseResponse<string?[]> GetSubject()
        {
            var res = new BaseResponse<string?[]>();
            res.Status = true;
            var schs = from sch in _context.Subjects
                       select sch;
            var scharray = schs.Select(x => x.SubjectName).ToArray();
            res.Data = scharray;
            return res;
        }
        public BaseResponse<List<Schedule>> GetMySchedule()
        {

            var res = new BaseResponse<List<Schedule>>();
            res.Status = true;
            try
            {
                var userEmail = _webHelpers.CurrentUser();

                //var Schedul = from Sche in _context.Schedules
                //              join bat in _context.Batches on Sche.BatcheId equals bat.Id
                //              join user in _context.CoachFrikaUsers on bat.TeachersId.ToString() equals user.Id
                //              where user.Email == userEmail
                //              select Sche;
                //var rr = Schedul.ToList();
                res.Message = "Successful";
                //res.Data = rr;
                return res;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
                return res;

            }
        }

        public async Task<BaseResponse<string>> SponsorAchild(SponsorDto model)
        {
            var res = new BaseResponse<string>();
            res.Status = true;
            try
            {
                var dto = new ChildSponsor();

                var phoneNumberValid = Validators.ValidatePhoneNumber(model.SponsorPhoneNumber);
                if (!phoneNumberValid)
                {
                    res.Message = "Phone number must be in the format: 0800 000 0000";
                    res.Status = false;
                    return res;
                }
                // Validate email format
                var emailValid = Validators.ValidateEmail(model.SponsorEmail);
                if (!emailValid)
                {
                    res.Message = "Invalid email format.";
                    res.Status = false;
                    return res;
                }
                dto.SponsorPhoneNumber = model.SponsorPhoneNumber;
                dto.SponsorEmail = model.SponsorEmail;

                dto.SponsorName = model.SponsorName;
                dto.NumbersOfChildren = model.NumbersOfChildren;
                var newsRepository = _unitOfWork.GetRepository<ChildSponsor>();
                await newsRepository.AddAsync(dto);
                await _unitOfWork.SaveChangesAsync();
                var mailSubject = _emailConfig.ContactTopic;
                var mailto = _emailConfig.MailTo.ToList();
                var body = await _emailService.ReadTemplate("emailrecieved");

                //inserting variable
                var messageToParse = new Dictionary<string, string>
                    {
                        { "{Fullname}", model.SponsorName},
                        { "{Phonenumber}", model.SponsorPhoneNumber},
                        { "{Email}", model.SponsorEmail},
                        { "{Message}", $"has request for Sponsor"},
                        { "{logo}", model.logoUrl},
                    };

                //  email notification
                var messageBody = body.ParseTemplate(messageToParse);
                var message = new Message(mailto, mailSubject, messageBody);
                //await _emailService.SendEmail(message);

                return res;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
                return res;

            }
        }
    }
}
