using CloudinaryDotNet;
using CoachFrika.APIs.Domin.BackgroundServices;
using CoachFrika.APIs.Domin.IServices;
using CoachFrika.APIs.Entity;
using CoachFrika.APIs.ViewModel;
using CoachFrika.Common;
using CoachFrika.Common.AppUser;
using CoachFrika.Common.AutoMapper;
using CoachFrika.Common.Enum;
using CoachFrika.Common.Extension;
using CoachFrika.Extensions;
using CoachFrika.Migrations;
using CoachFrika.Models;
using CoachFrika.Services;
using coachfrikaaaa.APIs.Entity;
using coachfrikaaaa.Common;
using Google.Apis.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Utilities;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using static CoachFrika.Common.LogingHandler.JwtServiceHandler;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CoachFrika.APIs.Domin.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly AppDbContext _context;
        public readonly IEmailService _emailService;
        public readonly IWebHelpers _webHelpers;
        private readonly UserManager<CoachFrikaUsers> _userManager;
        private readonly IPaystackService _paystackService;
        private readonly SubscriptionsConfigSettings _subscrib;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        public TeacherService(IUnitOfWork unitOfWork,
            AppDbContext context,
            IWebHelpers webHelpers, IEmailService emailService,
            UserManager<CoachFrikaUsers> userManager, IOptions<SubscriptionsConfigSettings> subscrib,
            IPaystackService paystackService, IBackgroundTaskQueue backgroundTaskQueue)
        {
            _context = context;
            _webHelpers = webHelpers;
            _userManager = userManager;
            _subscrib = subscrib.Value;
            _paystackService = paystackService;
            _backgroundTaskQueue = backgroundTaskQueue;
            _emailService = emailService;
        }
        public async Task<BaseResponse<string>> CreateStage1(TitleDto model)
        {

            var res = new BaseResponse<string>();
            res.Status = true;
            try
            {
                var user = _webHelpers.CurrentUser();
                if (user == null)
                {
                    res.Status = false;
                    res.Message = "User not found";
                    return res;
                }
                var detail = await _context.CoachFrikaUsers.FirstOrDefaultAsync(x => x.Email == user);
                detail.Title = model.Title;
                detail.Stages = 1;
                detail.ProfessionalTitle = model.ProfessionalTitle;
                await _context.SaveChangesAsync();
                return res;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
                return res;

            }
        }

        public async Task<BaseResponse<string>> CreateStage2(TeacherPhoneYearsDto model)
        {
            var res = new BaseResponse<string>();
            res.Status = true;
            try
            {
                var user = _webHelpers.CurrentUser();
                if (user == null)
                {
                    res.Status = false;
                    res.Message = "User not found";
                    return res;
                }

                var phoneNumberValid = Validators.ValidatePhoneNumber(model.PhoneNumber);
                if (!phoneNumberValid)
                {
                    res.Message = "Phone number must be in the format: 0800 000 0000";
                    res.Status = false;
                    return res;
                }

                // Check if the phone number already exists
                var detail = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.Email == user);
                if (detail == null)
                {
                    res.Message = "An account does not exists.";
                    res.Status = false;
                    return res;
                }

                var dateofwork = DateTime.Now.AddYears(-model.YearOfExperience);
                detail.NumbersOfStudents = model.NumberOfStudent.Value;
                detail.PhoneNumber = model.PhoneNumber;
                detail.YearStartExperience = dateofwork;
                detail.Stages = 2;
                await _userManager.UpdateAsync(detail);
                await _context.SaveChangesAsync();
                return res;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
                return res;

            }
        }

        public async Task<BaseResponse<string>> CreateStage3(DescriptionDto model)
        {
            var res = new BaseResponse<string>();
            res.Status = true;
            try
            {
                var user = _webHelpers.CurrentUser();
                if (user == null)
                {
                    res.Status = false;
                    res.Message = "User not found";
                    return res;
                }

                var detail = await _context.CoachFrikaUsers.FirstOrDefaultAsync(x => x.Email == user);
                detail.Description = model.Description;
                detail.Nationality = model.Nationality;
                detail.StateOfOrigin = model.StateOfOrigin;
                detail.Stages = 3;
                await _context.SaveChangesAsync();
                return res;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
                return res;

            }
        }

        public async Task<BaseResponse<string>> backStage(string email)
        {
            var res = new BaseResponse<string>();
            res.Status = true;
            try
            {
                var detail = await _context.CoachFrikaUsers.FirstOrDefaultAsync(x => x.Email == email);
                detail.SchoolName = null;
                detail.LocalGov = null;
                detail.Subject = null;
                detail.FacebookUrl = null;
                detail.TweeterUrl = null;
                detail.LinkedInUrl = null;
                detail.InstagramUrl = null;
                detail.hasPaid = false;
                detail.Stages = 3;
                await _context.SaveChangesAsync();
                return res;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
                return res;

            }
        }

        public async Task<BaseResponse<string>> CreateStage4(SocialMediaDto model)
        {
            var res = new BaseResponse<string>();
            res.Status = true;
            try
            {
                var user = _webHelpers.CurrentUser();
                if (user == null)
                {
                    res.Status = false;
                    res.Message = "User not found";
                    return res;
                }

                var detail = await _context.CoachFrikaUsers.FirstOrDefaultAsync(x => x.Email == user);
                detail.FacebookUrl = model.FacebookUrl;
                detail.TweeterUrl = model.TweeterUrl;
                detail.LinkedInUrl = model.LinkedInUrl;
                detail.InstagramUrl = model.InstagramUrl;
                detail.Stages = 4;
                await _context.SaveChangesAsync();
                return res;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
                return res;

            }
        }

        public async Task<BaseResponse<string>> CreateStage5(SchoolesdescriptionDto model)
        {
            var res = new BaseResponse<string>();
            res.Status = true;
            try
            {
                var user = _webHelpers.CurrentUser();
                if (user == null)
                {
                    res.Status = false;
                    res.Message = "User not found";
                    return res;
                }

                var detail = await _context.CoachFrikaUsers.FirstOrDefaultAsync(x => x.Email == user);
                detail.SchoolName = model.SchoolName;
                detail.LocalGov = model.LocalGov;
                detail.Subject = string.Join(", ", model.Subjects);
                detail.Stages = 5;
                await _context.SaveChangesAsync();
                return res;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
                return res;

            }
        }
        private async Task<BaseResponse<string>> processPaymentChecker(string refcode, string logo)
        {

            SentrySdk.CaptureMessage($"processPaymentChecker refcode: {refcode}", level: SentryLevel.Info);
            var res = new BaseResponse<string>();
            res.Status = true;
            try
            {
                var transactionUrl = await _paystackService.VerifyTransactionAsync(refcode, logo);

                if (transactionUrl == null)
                {
                    res.Message = "Error Occur: contact The Administration";
                    res.Status = false;
                    return res;
                }
                return res;

            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
                return res;

            }
        }
        public async Task<BaseResponse<string>> CreateStage6(SubscriptionsDto model)
        {
            var res = new BaseResponse<string>();
            res.Status = true;
            try
            {
                var loginUser = _webHelpers.CurrentUser();
                if (string.IsNullOrEmpty(loginUser))
                {
                    res.Status = false;
                    res.Message = "User not found";
                    return res;
                }
                var user = await _context.CoachFrikaUsers.FirstOrDefaultAsync(x => x.Email.ToLower() == loginUser.ToLower());
                if (user == null)
                {
                    res.Status = false;
                    res.Message = "User not found";
                    return res;
                }
                var existingPayment = await _context.Payment.FirstOrDefaultAsync(x => x.CreatedBy == loginUser && x.PaymentStatus == PaymentStatus.Pending);
                if (existingPayment != null)
                {
                    res.Status = false;
                    res.Message = "You have an existing payment process:Kindly reachout to Admin";
                    return res;
                }
                var sub = model.Subscription;
                var detail = await _context.CoachFrikaUsers.FirstOrDefaultAsync(x => x.Email == loginUser);
                var amount = model.Subscription switch
                {
                    Subscriptions.Intentional => _subscrib.Intentional,
                    Subscriptions.Phenomenal => _subscrib.Phenomenal,
                    Subscriptions.Transformational => _subscrib.Transformational,
                    _ => 0 // default value if none of the above match
                };

                detail.Subscriptions = model.Subscription;


                var payment = new Payment()
                {
                    Amount = amount,
                    Subscription = sub,
                    Paymentrefernce = Guid.NewGuid().ToString("N").Substring(10),
                    CreatedBy = loginUser,
                    CreatedDate = DateTime.UtcNow,
                    UserId = user.Id

                };
                user.Stages = 6;
                _context.Payment.Add(payment);
                _context.CoachFrikaUsers.Update(user);
                await _context.SaveChangesAsync();

                res.Message = "Payment directive has been sent to your email";
                res.Status = true;

                var bankName = "Access Bank";
                var accountNumber = "0045072769";
                var accountName = "Iyiola Afeez";
                var contactEmail = "Iyiola@gmail.com";
                var WhatsApp = "08164124811";
                var admin = "Iyiola@gmail.com";
                var subject = "Payment Invoice";
                var userbody = $@"Your request to pay for the {sub} has been received.
                                   Kindly proceed with your payment using the bank details
                                below, and use your reference code as the payment description:

                                Bank Name: {bankName}
                                Account Number: {accountNumber}
                                Account Name: {accountName}

                                Once payment is made, please send proof of payment 
                                to {contactEmail} or {WhatsApp} to complete your subscription.

                                 Thank you for choosing us!";

                var bodyTemplate = await _emailService.ReadTemplate("paymentRequest");
                //inserting variable
                //inserting variable
                var UsermessageToParse = new Dictionary<string, string>
                    {
                        { "{Fullname}", user.FullName},
                        { "{Message}", userbody},
                        { "{logo}", model.Logo},
                    };

                //  email notification
                var UsermessageBody = bodyTemplate.ParseTemplate(UsermessageToParse);
                var message = new Message(new List<string> { loginUser }, subject, UsermessageBody);

                await _emailService.SendEmail(message);

                var Adminsubject = "Payment Notification";
                var Adminbody = $@"A user has requested to pay for the {sub} package.

                                    Please find the details below:

                                    Name: {user.FullName}
                                    Subscription: {sub}
                                    Phone Number: {user.PhoneNumber}
                                    Email: {user.Email}

                                    Kindly reach out to follow up and verify the payment once it is completed.";

                var adminbodyTemplate = await _emailService.ReadTemplate("paymentRequest");
                //inserting variable
                var messageToParse = new Dictionary<string, string>
                    {
                        { "{Fullname}", user.FullName},
                        { "{Message}", Adminbody},
                        { "{logo}", model.Logo},
                    };

                //  email notification
                var messageBody = bodyTemplate.ParseTemplate(messageToParse);
                var adminmessage = new Message(new List<string> { admin }, Adminsubject, messageBody);

                await _emailService.SendEmail(adminmessage);
                return res;

            }
            catch (Exception ex)
            {
                SentrySdk.CaptureMessage($"Message :{ex.Message},StackTrace:{ex.StackTrace}", level: SentryLevel.Info);
                res.Message = "Oops! Error Occur: Kindly try again later";
                res.Status = false;
                return res;

            }
        }

        public BaseResponse<List<ProfileDto>> MyTeachers(GetTeachers query)
        {
            var userId = _webHelpers.CurrentUserId();
            var res = new BaseResponse<List<ProfileDto>>();
            res.Status = true;
            try
            {
                var day = DateTime.Now.Day;
                // Apply filters based on the query parameters
                var cos = from teachers in _context.CoachFrikaUsers
                          where teachers.CoachId == userId
                          select new ProfileDto
                          {
                              Id = teachers.Id,
                              Title = teachers.Title,
                              FullName = teachers.FullName     // Using DateTime.MinValue if EndDate is null
                          };

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

        public BaseResponse<List<SchedulesViewModel>> GetMySchedule(GetSchedules query)
        {
            var user = _webHelpers.CurrentUser();
            var res = new BaseResponse<List<SchedulesViewModel>>();
            res.Status = true;
            var teach = _context.CoachFrikaUsers.FirstOrDefault(x => x.Email == user);
            if (teach == null)
            {
                res.Message = "User not found";
                res.Status = false;
                return res;
            };
            if (teach.CoachId == null)
            {
                res.Message = "No Schedule Found: Kindly Select A Coach";
                res.Status = false;
                return res;

            }
            try
            {
                var day = DateTime.Now.Date;
                // Apply filters based on the query parameters
                var cos = from schedule in _context.Schedule
                          where (string.IsNullOrEmpty(query.Title) || schedule.Title.Contains(query.Title))
                                && (query.status == Common.Enum.ScheduleStatus.ongoing
                                        ? (schedule.StartDate.Value.Date == day)
                                        : query.status == Common.Enum.ScheduleStatus.past
                                            ? (schedule.StartDate.Value.Date < day)
                                            : query.status == Common.Enum.ScheduleStatus.comingsoon
                                            ? (schedule.StartDate.Value.Date > day)
                                            : (schedule.StartDate.Value.Date < day && !schedule.TeacherAttended))
                               && schedule.CoachId == teach.CoachId
                               && schedule.Focus == teach.Subscriptions
                          select new SchedulesViewModel
                          {
                              Id = schedule.Id,
                              Title = schedule.Title,
                              Focus = schedule.Focus.ToString(),
                              MeetingUrl = schedule.MeetingLink,
                              StartDate = schedule.StartDate ?? DateTime.MinValue,  // Using DateTime.MinValue if StartDate is null
                              EndDate = schedule.EndDate ?? DateTime.MinValue      // Using DateTime.MinValue if EndDate is null
                          };

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

        public async Task<BaseResponse<string>> SelectCoach(Guid CoachId)
        {

            var res = new BaseResponse<string>();
            res.Status = true;
            try
            {
                var user = _webHelpers.CurrentUser();
                var teach = _context.CoachFrikaUsers.FirstOrDefault(x => x.Email == user);
                if (teach == null)
                {
                    res.Status = false;
                    res.Message = "User not found";
                    return res;
                }

                var coach = _context.CoachFrikaUsers.FirstOrDefault(x => x.Id == CoachId.ToString());
                if (coach == null)
                {
                    res.Status = false;
                    res.Message = "Coach not found";
                    return res;
                }

                teach.CoachId = CoachId.ToString();
                await _context.SaveChangesAsync();
                return res;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
                return res;

            }
        }
        public async Task<BaseResponse<ProfileDto>> GetTeacherById(string Id)
        {
            var res = new BaseResponse<ProfileDto>();
            res.Status = true;
            try
            {
                var coach = await _context.CoachFrikaUsers.FirstOrDefaultAsync(x => x.Id == Id);
                if (coach == null)
                {
                    res.Message = "Teacher not found";
                    res.Status = false;
                    return res;
                }

                var profile = ProfileMapper.MapToProfileDto(coach);
                res.Data = profile;
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

        public BaseResponse<List<GetTeacherRecommendationResponse>> Recommendations(GetTeacherRecommendations query)
        {
            var userId = _webHelpers.CurrentUserId();
            var res = new BaseResponse<List<GetTeacherRecommendationResponse>>();
            res.Status = true;
            try
            {
                // Apply filters based on the query parameters
                var cos = from rec in _context.Recommendations
                          join coach in _context.CoachFrikaUsers on rec.CoachId equals coach.Id
                          join schd in _context.Schedule on rec.ScheduleId equals schd.Id.ToString()
                          where rec.TeacherId == userId &&
                          (string.IsNullOrEmpty(query.ScheduleTitle) || schd.Title.Contains(query.ScheduleTitle))
                          select new GetTeacherRecommendationResponse
                          {
                              Id = rec.Id.ToString(),
                              CoachName = coach.FullName,
                              ScheduleTitle = schd.Title,
                              Recommendation = rec.Recommendation,
                              ScheduleId = rec.ScheduleId,
                              Remark = rec.TeacherRemark,

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
        public async Task<BaseResponse<string>> RecommendationRemark(TeachersRemark model)
        {
            var res = new BaseResponse<string>();
            res.Status = true;
            try
            {
                var recomm = await _context.Recommendations.Where(x => x.Id == model.Id).FirstOrDefaultAsync();

                if (recomm == null)
                {
                    res.Status = false;
                    res.Message = "schedule not found";
                    return res;
                }
                recomm.TeacherRemark = model.TeacherRemark;

                _context.Recommendations.Update(recomm);
                await _context.SaveChangesAsync();
                return res;

            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
                return res;
            }
        }

        public BaseResponse<List<ProfileDto>> GetTeachers(GetTeachersSearch query)
        {
            var res = new BaseResponse<List<ProfileDto>>();
            res.Status = true;
            try
            {
                var cos = from user in _context.CoachFrikaUsers
                          where (user.Role == Roles.Teacher)
                          && (
                    (query.OnboardingStatus == null) ||
                    (query.OnboardingStatus == OnboardingStatus.PendindApproval && user.Stages == 4 && !user.hasPaid) ||
                    (query.OnboardingStatus == OnboardingStatus.Approved && user.Stages == 4 && user.hasPaid) ||
                     (query.OnboardingStatus == OnboardingStatus.Ongoing && user.Stages < 4 && !user.hasPaid)
                )
                          select new ProfileDto
                          {
                              Id = user.Id,
                              Title = user.Title,
                              FullName = user.FullName,
                              ProfessionalTitle = user.ProfessionalTitle,
                              NumbersOfStudents = user.NumbersOfStudents,
                              Description = user.Description,
                              Email = user.Email,
                              PhoneNumber = user.PhoneNumber,
                              Role = user.Role,
                              Stages = user.Stages,
                              hasPaid = user.hasPaid,
                              SchoolName = user.SchoolName,



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
    }
}
