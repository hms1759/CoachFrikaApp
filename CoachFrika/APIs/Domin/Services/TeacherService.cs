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
            IWebHelpers webHelpers,
            UserManager<CoachFrikaUsers> userManager, IOptions<SubscriptionsConfigSettings> subscrib,
            IPaystackService paystackService, IBackgroundTaskQueue backgroundTaskQueue)
        {
            _context = context;
            _webHelpers = webHelpers;
            _userManager = userManager;
            _subscrib = subscrib.Value;
            _paystackService = paystackService;
            _backgroundTaskQueue = backgroundTaskQueue;
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
                var user = _webHelpers.CurrentUser();
                if (string.IsNullOrEmpty(user))
                {
                    res.Status = false;
                    res.Message = "User not found";
                    return res;
                }
                var sub = model.Subscription;
                var detail = await _context.CoachFrikaUsers.FirstOrDefaultAsync(x => x.Email == user);
                var amount = model.Subscription switch
                {
                    Subscriptions.Intentional => _subscrib.Intentional,
                    Subscriptions.Phenomenal => _subscrib.Phenomenal,
                    Subscriptions.Transformational => _subscrib.Transformational,
                    _ => 0 // default value if none of the above match
                };

                detail.Subscriptions = model.Subscription;
                var transactionUrl = await _paystackService.InitializeTransactionAsync(amount, detail.Email);

                if (transactionUrl != null)
                {
                    var obj = transactionUrl.Data;
                    if (transactionUrl.Status)
                    {
                        detail.Subscriptions = model.Subscription;

                        var payment = new Payment()
                        {
                            Amount = amount,
                            Subscription = sub,
                            Paymentrefernce = obj.reference,
                            CreatedBy = user,
                            CreatedDate = DateTime.UtcNow

                        };
                        _context.Payment.Add(payment);
                        await _context.SaveChangesAsync();
                        res.Data = obj.authorization_url;

                        // Queue the background task to process payment after 15 minutes
                        await _backgroundTaskQueue.QueueBackgroundWorkItemAsync(async token =>
                        {
                            SentrySdk.CaptureMessage($"QueueBackgroundWorkItem reference : {obj.reference}, logo {model.Logo} url.", level: SentryLevel.Info);
                            await Task.Delay(TimeSpan.FromMinutes(15), token); // Delay for 15 minutes
                            await processPaymentChecker(obj.reference, model.Logo);
                        });
                        return res;
                    }


                    SentrySdk.CaptureMessage($"transactionUrl Error : {transactionUrl.Message}", level: SentryLevel.Info);
                    res.Message = transactionUrl.Message;
                    res.Status = false;
                    return res;
                }


                res.Message = "Error Occur: contact The Administration";
                res.Status = false;
                return res;

            }
            catch (Exception ex)
            {
                SentrySdk.CaptureMessage($"Message :{ex.Message},StackTrace:{ex.StackTrace}", level: SentryLevel.Info);
                res.Message = ex.Message;
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
                                            ? (schedule.StartDate.Value.Date > day)
                                            : query.status == Common.Enum.ScheduleStatus.comingsoon
                                            ? (schedule.StartDate.Value.Date < day)
                                            : (schedule.StartDate.Value.Date > day && !schedule.TeacherAttended))
                                  && schedule.CoachId == teach.CoachId
                               && schedule.Focus == teach.Subscriptions
                                && (query.Scheduled == null || schedule.StartDate.Value.Date == query.Scheduled.Value.Date)
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

        public BaseResponse<List<GetCoachesRecommendationResponse>> Recommendations(GetTeacherRecommendations query)
        {
            var userId = _webHelpers.CurrentUserId();
            var res = new BaseResponse<List<GetCoachesRecommendationResponse>>();
            res.Status = true;
            try
            {
                // Apply filters based on the query parameters
                var cos = from rec in _context.Recommendations
                          join coach in _context.CoachFrikaUsers on rec.CoachId equals coach.Id
                          join schd in _context.Schedule on rec.ScheduleId equals schd.Id.ToString()

                          where rec.TeacherId == userId
                          && (string.IsNullOrEmpty(query.ScheduleTitle) || schd.Title.Contains(query.ScheduleTitle))
                          select new GetCoachesRecommendationResponse
                          {
                              Id = rec.Id.ToString(),
                              CoachName = coach.FullName,
                              ScheduleTitle = schd.Title,
                              Recommendation = rec.Recommendation,
                              ScheduleId = rec.ScheduleId,

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

    }
}
