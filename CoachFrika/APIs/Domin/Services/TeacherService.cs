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
using Google.Apis.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        public TeacherService(IUnitOfWork unitOfWork,
            AppDbContext context,
            IWebHelpers webHelpers,
            UserManager<CoachFrikaUsers> userManager)
        {
            _context = context;
            _webHelpers = webHelpers;
            _userManager = userManager;
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
                detail.Stages += 1;
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
                    throw new ArgumentException("Phone number must be in the format: 0800 000 0000");
                }

                // Check if the phone number already exists
                var detail = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.Email == user);
                if (detail == null)
                {
                    throw new ArgumentException("An account does not exists.");
                }

                var dateofwork = DateTime.Now.AddYears(model.YearOfExperience);
                detail.NumbersOfStudents = model.NumberOfStudent.Value;
                detail.PhoneNumber = model.PhoneNumber;
                detail.YearStartExperience = dateofwork;
                detail.Stages += 1;
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
                detail.Stages += 1;
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
                detail.Stages += 1;
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
                detail.Stages += 1;
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
        public async Task<BaseResponse<string>> CreateStage6(SubscriptionsDto model)
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

                detail.Stages += 1;
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
            var teach = _context.CoachFrikaUsers.FirstOrDefault(x => x.Email == user);
            if (teach == null) { };
            var res = new BaseResponse<List<SchedulesViewModel>>();
            res.Status = true;
            try
            {
                var day = DateTime.Now.Day;
                // Apply filters based on the query parameters
                var cos = from schedule in _context.Schedule
                          where (string.IsNullOrEmpty(query.Title) || schedule.Title.Contains(query.Title))
                               && query.status == Common.Enum.ScheduleStatus.ongoing ? (schedule.StartDate.Value.Day == day) : query.status == Common.Enum.ScheduleStatus.past ? (schedule.StartDate.Value.Day > day) : (schedule.StartDate.Value.Day < day)// Assuming you filter based on a scheduled date
                               && schedule.CoachId == teach.CoachId && schedule.Focus == teach.Subscriptions
                          select new SchedulesViewModel
                          {
                              Id = schedule.Id,
                              Title = schedule.Title,
                              Focus = schedule.Focus.ToString(),
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
    }
}
