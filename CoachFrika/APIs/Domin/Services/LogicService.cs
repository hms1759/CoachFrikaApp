using CoachFrika.APIs.Domin.IServices;
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

        public async Task<string> ContactUs(ContactUsDto model)
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
                        { "{logo}", model.logoUrl},
                    };

                //  email notification
                var messageBody = body.ParseTemplate(messageToParse);
                var message = new Message(mailto, mailSubject, messageBody);
                await _emailService.SendEmail(message);

                return "Successful";
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);

            }
        }

        public async Task<PublicCountDto> GetPublicCount()
        {
            var schcount = await _context.Schools.ToListAsync();
            var usercount = await _context.CoachFrikaUsers.Where(x => x.Role == 1 || x.Role == 0).ToListAsync();
            //if (usercount.Count() >1)
            //    throw new NotImplementedException("User not found");
            var dto = new PublicCountDto();
            // Get count of users with Coach role
            dto.CoachesCount = usercount.Where(x => x.Role == 1).Count();
            // Get count of users with Teacher role
            var Teachers = usercount.Where(x => x.Role == 0).ToList();
            dto.TeachersCount = Teachers.Count;
            dto.StudentCount = Teachers.Sum(x => x.NumbersOfStudents);
            dto.SchoolCount = schcount.Count;
            return dto;
        }

        public async Task<string> NewSubscription(SubscriptionDto modle)
        {
            try
            {
                var dto = new NewsSubscription();
                dto.Email = modle.Email;
                var newsRepository = _unitOfWork.GetRepository<NewsSubscription>();
                await newsRepository.AddAsync(dto);
                await _unitOfWork.SaveChangesAsync();
                return "Successful";
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);

            }
        }

        public async Task<string> CreateSchool(string school)
        {
            try
            {
                var dto = new Schools();
                dto.School = school;
                var schRepository = _unitOfWork.GetRepository<Schools>();
                await schRepository.AddAsync(dto);
                await _unitOfWork.SaveChangesAsync();
                return "Successful";
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);

            }
        }
        public async Task<string> CreateSubject(List<string> sub)
        {
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
                return "Successful";
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);

            }
        }

        public async Task<TeachersDTo> GetUserById(Guid Id)
        {
            var userRepo = _unitOfWork.GetRepository<CoachFrikaUsers>();
            var user = await userRepo.GetByIdAsync(Id);
            return await GetUserByMail(user?.Email);
        }

        public async Task<TeachersDTo> GetUserDetails()
        {
            var email = _webHelpers.CurrentUser();
            return await GetUserByMail(email);
        }
        public async Task<TeachersDTo> GetUserByMail(string email)
        {
            var year = DateTime.Now.Year;
            try
            {

                var user = from users in _context.CoachFrikaUsers
                           join sch in _context.Schools on users.SchoolId equals sch.Id
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
                               YearOfExperience = year - users.YearStartExperience.Year,
                               School = sch.School,
                               Subjects = users.Subjects.Select(x => x.SubjectName).ToArray()
                           };

                var teacherDto = await user.FirstOrDefaultAsync();
                return teacherDto;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public string?[] GetSchool()
        {
            var schs = from sch in _context.Schools
                       select sch;
            var scharray =schs.Select(x => x.School).ToArray();
            return scharray;
        }

        public string?[] GetSubject()
        {
            var schs = from sch in _context.Subjects
                       select sch;
            var scharray = schs.Select(x => x.SubjectName).ToArray();
            return scharray;
        }
        public List<Schedules> GetMySchedule()
        {
            try
            {
                var userEmail = _webHelpers.CurrentUser();

                var Schedul = from Sche in _context.Schedules
                          join bat in _context.Batches on Sche.BatcheId equals bat.Id
                          join user in _context.CoachFrikaUsers on bat.TeachersId equals user.Id
                          where user.Email == userEmail
                              select Sche;
                return Schedul.ToList();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);

            }
        }
    }
}
