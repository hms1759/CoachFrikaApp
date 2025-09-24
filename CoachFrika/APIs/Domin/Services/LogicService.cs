using CoachFrika.APIs.Domin.IServices;
using CoachFrika.APIs.Entity;
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
        public LogicService(IUnitOfWork unitOfWork, IOptions<EmailConfigSettings> emailConfig,
            AppDbContext context, IEmailService emailService, IWebHelpers webHelpers)
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
                var newsRepository = _unitOfWork.GetRepository<SchoolEnrollmentRequest>();
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
                await _emailService.SendEmail(message);

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
                if (IsValidEmail(model.Email) && await HasMxRecord(model.Email))
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

                    return res;
                }
                else
                {
                    res.Message = "Invalid email format.";
                    res.Status = false;
                    return res;
                }
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
                var schcount = await _context.SchoolEnrollmentRequest.Where(x => x.isSubscribed).ToListAsync();
                var usercount = await _context.CoachFrikaUsers.Where(x => x.Role == Roles.Coach || x.Role == Roles.Teacher).ToListAsync();
                //if (usercount.Count() >1)
                //    res.Message = "User not found");
                var dto = new PublicCountDto();
                // Get count of users with Coach role
                dto.CoachesCount = usercount.Where(x => x.Role == Roles.Coach).Count();
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
            {
                // Validate email format
                if (IsValidEmail(model.Email) && await HasMxRecord(model.Email))
                {
                    var dto = new NewsSubscription();
                    dto.Email = model.Email;
                    var newsRepository = _unitOfWork.GetRepository<NewsSubscription>();
                    await newsRepository.AddAsync(dto);
                    await _unitOfWork.SaveChangesAsync();
                    res.Message = "Successful";
                    return res;
                }
                else
                {
                    res.Message = "Invalid email format.";
                    res.Status = false;
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
                return res;

            }
        }
        public async Task<BaseResponse<string>> CreateSubject(List<SubjectDTO> subRequest)
        {
            var userId = _webHelpers.CurrentUserId();
            var res = new BaseResponse<string>();
            res.Status = true;
            try
            {
                var subRepository = _unitOfWork.GetRepository<Subjects>();
                var dtoList = new List<Subjects>();
                foreach (var subItem in subRequest)
                {
                    var existSub = await _context.Subjects
    .FirstOrDefaultAsync(x => x.TeachersId == userId &&
                              x.SubjectId == subItem.SubjectId);

                    if (existSub != null)
                    {
                        res.Message = $"Subject with the name {subItem.Subject}: Already exist";
                        res.Status = false;
                        return res;

                    }
                    var sub = new Subjects();
                    sub.TeachersId = userId;
                    sub.SubjectId = subItem.SubjectId;
                    sub.Subject = subItem.Subject;

                    dtoList.Add(sub);
                }

                await subRepository.AddRangeAsync(dtoList);
                var studentList = _context.Students.Where(x => x.TeachersId == userId);
                var liststd = new List<StudentScoreSheet>();
                foreach (var sub in dtoList)
                {
                    if (studentList.Any())
                    {
                        foreach (var std in studentList)
                        {
                            var scoreSheet = new StudentScoreSheet()
                            {
                                TeachersId = sub.TeachersId,
                                StudentId = std.Id.ToString(),
                                SubjectId = sub.Id.ToString()
                            };
                            liststd.Add(scoreSheet);
                        }
                        await _context.StudentScoreSheet.AddRangeAsync(liststd);
                    }
                }
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

        //public BaseResponse<string?[]> GetSubject()
        //{
        //    var id = _webHelpers.CurrentUserId();
        //    var res = new BaseResponse<string?[]>();
        //    res.Status = true;
        //    var schs = from sch in _context.Subjects
        //               where sch.TeachersId == id
        //               select sch;
        //    var scharray = schs.Select(x => x.SubjectName).ToArray();
        //    res.Data = scharray;
        //    return res;
        //}

        public BaseResponse<List<Subjects>> GetSubject()
        {
            var schsEmp = new List<Subjects>();
            var id = _webHelpers.CurrentUserId();
            var res = new BaseResponse<List<Subjects>>();
            res.Status = true;
            var schs = from sch in _context.Subjects
                       where sch.TeachersId == id
                       select sch;
            res.Data = schs.Any() ? schs.ToList() : schsEmp;
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
        public async Task<bool> HasMxRecord(string email)
        {
            var domain = email.Split('@').LastOrDefault();
            if (string.IsNullOrEmpty(domain)) return false;

            var lookup = new DnsClient.LookupClient();
            var result = await lookup.QueryAsync(domain, DnsClient.QueryType.MX);
            return result.Answers.MxRecords().Any();
        }

        public async Task<BaseResponse<string>> CreateScheme(CreateSchemesDto model)
        {
            var res = new BaseResponse<string>();
            res.Status = true;
            var existScheme = _context.Schemes.Any(x => x.Plan == model.Plan && x.Title == model.Title && x.WeekNumber == model.WeekNumber);
            if (existScheme)
            {
                res.Message = "Scheme Already created";
                res.Status = false;
                return res;
            }
            var newScheme = new Schemes
            {
                Title = model.Title,
                Plan = model.Plan,
                WeekNumber = model.WeekNumber,
                Description = model.Description

            };
            _context.Schemes.Add(newScheme);
            await _context.SaveChangesAsync();
            return res;
        }

        public BaseResponse<List<ResponseSchemesDto>> GetAllScheme(GetSchemeSearch query)
        {
            var res = new BaseResponse<List<ResponseSchemesDto>>();
            res.Status = true;
            try
            {
                // Apply filters based on the query parameters
                var cos = from rec in _context.Schemes
                          where (query.Title == null || rec.Title.Contains(query.Title))
                          && (query.Plans == null || rec.Plan == query.Plans)
                          && (query.WeekNumber == null || rec.WeekNumber == query.WeekNumber)
                          select new ResponseSchemesDto
                          {
                              Id = rec.Id,
                              Description = rec.Description,
                              Title = rec.Title,
                              Plan = rec.Plan.ToString(),
                              CreatedBy = rec.ModifiedBy ?? rec.CreatedBy,
                              WeekNumber = rec.WeekNumber

                          };

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

        /// <summary>
        /// this will be use by coach
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public BaseResponse<List<Schemes>> GetAllSchemeByPlan(Subscriptions? plan)
        {
            var res = new BaseResponse<List<Schemes>>();
            res.Status = true;
            try
            {
                // Apply filters based on the query parameters
                var cos = from rec in _context.Schemes
                          where (plan == null || rec.Plan == plan)
                          select rec;

                // Set the response data
                res.Data = cos != null ? cos.ToList() : null;
                return res;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
                return res;

            }

        }

        public async Task<BaseResponse<string>> EditScheme(EditSchemesDto model)
        {
            var res = new BaseResponse<string>();
            res.Status = true;
            var existScheme = await _context.Schemes.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (existScheme is null)
            {
                res.Message = "Scheme does not exist ";
                res.Status = false;
                return res;
            }

            existScheme.Title = model.Title;
            existScheme.Plan = model.Plan;
            existScheme.WeekNumber = model.WeekNumber;
            existScheme.Description = model.Description;

            _context.Schemes.Update(existScheme);
            await _context.SaveChangesAsync();
            return res;
        }

        public async Task<BaseResponse<string>> DeleteScheme(Guid Id)
        {
            var res = new BaseResponse<string>();
            res.Status = true;
            var existScheme = await _context.Schemes.FirstOrDefaultAsync(x => x.Id == Id);
            if (existScheme is null)
            {
                res.Message = "Scheme does not exist ";
                res.Status = false;
                return res;
            }

            _context.Schemes.Remove(existScheme);
            await _context.SaveChangesAsync();
            return res;
        }
        public async Task<BaseResponse<List<UserTrendDto>>> GetUserTrend(TrendRequest trendRequest)
        {
            var res = new BaseResponse<List<UserTrendDto>> { Status = true };
            var query = _context.CoachFrikaUsers.AsQueryable();

            DateTime today = DateTime.UtcNow.Date;
            DateTime startDate = today, endDate = today;

            switch (trendRequest.PeriodType)
            {
                case PeriodType.Week:
                    startDate = today.AddDays(-(int)today.DayOfWeek);
                    endDate = startDate.AddDays(7);
                    query = query.Where(u => u.CreatedDate >= startDate && u.CreatedDate < endDate);
                    break;

                case PeriodType.Month:
                    startDate = new DateTime(today.Year, today.Month, 1);
                    endDate = startDate.AddMonths(1);
                    query = query.Where(u => u.CreatedDate >= startDate && u.CreatedDate < endDate);
                    break;

                case PeriodType.Year:
                    startDate = new DateTime(today.Year, 1, 1);
                    endDate = startDate.AddYears(1);
                    query = query.Where(u => u.CreatedDate >= startDate && u.CreatedDate < endDate);
                    break;

                case PeriodType.Range:
                    if (trendRequest.StartDate.HasValue && trendRequest.EndDate.HasValue)
                    {
                        startDate = trendRequest.StartDate.Value.Date;
                        endDate = trendRequest.EndDate.Value.Date.AddDays(1);
                        query = query.Where(u => u.CreatedDate >= startDate && u.CreatedDate < endDate);
                    }
                    break;
            }

            List<UserTrendDto> result;

            if (trendRequest.PeriodType == PeriodType.Year)
            {
                // Group by month
                var grouped = await query
                    .GroupBy(u => new { u.CreatedDate.Year, u.CreatedDate.Month })
                    .Select(g => new
                    {
                        g.Key.Year,
                        g.Key.Month,
                        Teachers = g.Count(x => x.Role == Roles.Teacher),
                        Coaches = g.Count(x => x.Role == Roles.Coach)
                    })
                    .ToListAsync();

                // Fill all months Jan–Dec
                result = Enumerable.Range(1, 12)
                    .Select(m =>
                    {
                        var found = grouped.FirstOrDefault(x => x.Month == m && x.Year == startDate.Year);
                        return new UserTrendDto
                        {
                            Label = $"{startDate.Year}-{m:D2}", // 2025-01
                            Teachers = found?.Teachers ?? 0,
                            Coaches = found?.Coaches ?? 0
                        };
                    })
                    .ToList();
            }
            else
            {
                // Group by day
                var grouped = await query
                    .GroupBy(u => u.CreatedDate.Date)
                    .Select(g => new
                    {
                        Date = g.Key,
                        Teachers = g.Count(x => x.Role == Roles.Teacher),
                        Coaches = g.Count(x => x.Role == Roles.Coach)
                    })
                    .ToListAsync();

                // Fill all days in range
                result = Enumerable.Range(0, (endDate - startDate).Days)
                    .Select(offset =>
                    {
                        var date = startDate.AddDays(offset);
                        var found = grouped.FirstOrDefault(x => x.Date == date);
                        return new UserTrendDto
                        {
                            Label = date.ToString("yyyy-MM-dd"),
                            Teachers = found?.Teachers ?? 0,
                            Coaches = found?.Coaches ?? 0
                        };
                    })
                    .ToList();
            }

            res.Data = result;
            res.Message = "Successful";
            return res;
        }
    }
}
