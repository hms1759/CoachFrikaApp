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
using System.Text;
using System.Text.RegularExpressions;
using static CoachFrika.Common.LogingHandler.JwtServiceHandler;

namespace CoachFrika.APIs.Domin.Services
{
    public class CousesService : ICousesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        private readonly EmailConfigSettings _emailConfig;
        public readonly IEmailService _emailService;
        public readonly IWebHelpers _webHelpers;
        public CousesService(IUnitOfWork unitOfWork, IOptions<EmailConfigSettings> emailConfig, AppDbContext context, IEmailService emailService, IWebHelpers webHelpers)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _emailConfig = emailConfig.Value;
            _emailService = emailService;
            _webHelpers = webHelpers;
        }

        public async Task<BaseResponse<string>> CreateSchedule(CreateScheduleDto model)
        {

            var user = _webHelpers.CurrentUser();
            var userId = _webHelpers.CurrentUserId();
            var res = new BaseResponse<string>();
            res.Status = true;
            try
            {
                var dto = new Schedule();
                dto.Title = model.Title;
                dto.Focus = model.Focus;
                dto.StartDate = model.Scheduled;
                dto.MeetingLink = model.MeetingUrl;
                dto.CreatedBy = user;
                dto.CoachId = userId;
                dto.EndDate = model.DurationType == Common.Enum.DurationType.Hour ? model.Scheduled.AddHours(model.Duration) : model.Scheduled.AddMinutes(model.Duration);
                var schRepository = _unitOfWork.GetRepository<Schedule>();
                await schRepository.AddAsync(dto);
                await _unitOfWork.SaveChangesAsync();
                return res;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
                return res;

            }
        }
        public BaseResponse<List<SchedulesViewModel>> GetCoachSchedule(GetSchedules query)
        {
            var user = _webHelpers.CurrentUser();
            var res = new BaseResponse<List<SchedulesViewModel>>();
            res.Status = true;
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
                                            : (schedule.StartDate.Value.Date < day && !schedule.CoachAttended))
                                 && schedule.CreatedBy == user
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


        public BaseResponse<string> EditSchedule(EditScheduleDto model)
        {

            var res = new BaseResponse<string>();
            res.Status = true;
            try
            {
                var dto =  _context.Schedule.FirstOrDefault(x => x.Id == model.Id);
                dto.Title = model.Title;
                dto.Focus = model.Focus;
                dto.StartDate = model.Scheduled;
                dto.MeetingLink = model.MeetingUrl;
                dto.EndDate = model.DurationType == Common.Enum.DurationType.Hour ? model.Scheduled.AddHours(model.Duration) : model.Scheduled.AddMinutes(model.Duration);
               
                 _context.SaveChanges();
                return res;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
                return res;

            }
            throw new NotImplementedException();
        }
        //public async Task<BaseResponse<string>> CreateBatches(BatchesDto model)
        //{
        //    var res = new BaseResponse<string>();
        //    res.Status = true;
        //    try
        //    {
        //        var dto = new Batches();
        //        dto.Title = model.Title;
        //        var schRepository = _unitOfWork.GetRepository<Batches>();
        //        await schRepository.AddAsync(dto);
        //        await _unitOfWork.SaveChangesAsync();
        //        return res;
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Message = ex.Message;
        //        res.Status = false;
        //        return res;

        //    }
        //}

        public BaseResponse<List<CoursesViewModel>> GetCourses()
        {

            var res = new BaseResponse<List<CoursesViewModel>>();
            res.Status = true;
            try
            {
                var cos = from course in _context.Schedule.AsNoTracking()
                          select new CoursesViewModel
                          {
                              Id = course.Id,
                              //CourseTitle = course.CourseTitle
                          };
                var rr = cos.ToList();
                res.Data = rr;
                return res;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
                return res;

            }

        }

        public BaseResponse<List<BatchesViewModel>> GetBatches()
        {

            var res = new BaseResponse<List<BatchesViewModel>>();
            res.Status = true;
            try
            {
                //var bat = from batche in _context.Batches.AsNoTracking()
                //          select new BatchesViewModel
                //          {
                //              Id = batche.Id,
                //              Title = batche.Title
                //          };
                //var rr = bat.ToList();
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

       //public async Task<BaseResponse<string>> CreateSchedule(SchedulesDto model)
         ////{
        //    var res = new BaseResponse<string>();
        //    res.Status = true;
        //    try
        //    {
        //        var BatchRepository = _unitOfWork.GetRepository<Batches>();
        //        var checkBatch = await BatchRepository.GetByIdAsync(model.BatcheId);
        //        if (checkBatch == null)
        //        {
        //            throw new NotImplementedException($"Batch with {model.BatcheId} not found");

        //        }
        //        var courseRepository = _unitOfWork.GetRepository<Batches>();
        //        var courseBatch = await courseRepository.GetByIdAsync(model.CourseId);
        //        if (courseBatch == null)
        //        {
        //            throw new NotImplementedException($"Course with {model.CourseId} not found");

        //        }
        //        var dto = new Schedules();
        //        dto.Description = model.Description;
        //        dto.Subject = model.Subject;
        //        dto.MaterialUrl= model.MaterialUrl;
        //        dto.Schedule = model.Schedule;
        //        dto.BatcheId = model.BatcheId;
        //        //dto.CourseId = model.CourseId;
        //        var schRepository = _unitOfWork.GetRepository<Schedules>();
        //        await schRepository.AddAsync(dto);
        //        await _unitOfWork.SaveChangesAsync();

        //        return res;
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Message = ex.Message;
        //        res.Status = false;
        //        return res;

        //    }
        //}
        public BaseResponse<List<Schedule>> GetMySchedule()
        {
            var res = new BaseResponse<List<Schedule>>();
            res.Status = true;
            try
            {
                var user = _webHelpers.CurrentUser();
                var bat = from Schedule in _context.Schedule
                          where Schedule.CreatedBy == user
                          select Schedule;
                var rr = bat.ToList();
                // res.Data = rr;
                return res;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
                return res;

            }
        }

        public BaseResponse<string> AttendSchedle(Guid Id)
        {
            var userRole = _webHelpers.CurrentUserRole();
            var res = new BaseResponse<string>();
            res.Status = true;
            var schedule =  _context.Schedule.FirstOrDefault(x => x.Id == Id);
            if (schedule == null)
            {
                res.Message = "schedule not found";
                res.Status = false;
                return res;
            };
            if (DateTime.Now > schedule.StartDate.Value.AddMinutes(-15) && DateTime.Now < schedule.EndDate.Value)
            {
                if (userRole.Equals("Coach"))
                { schedule.CoachAttended = true; }
                else
                { schedule.TeacherAttended = true; }
            }

            res.Message = "Attendance Successfully Marked";
            res.Status = true;
            return res;
        }

        public BaseResponse<SchedulesDto> GetScheduleById(Guid Id)
        {
            var res = new BaseResponse<SchedulesDto>();
            res.Status = true;
            try
            {
                var sch =  _context.Schedule.FirstOrDefault(x => x.Id == Id);
                if (sch == null)
                {
                    res.Message = "schedule not found";
                    res.Status = false;
                    return res;
                };
                var respose = new SchedulesDto()
                {
                    Id = sch.Id,
                    Title = sch.Title,
                    Focus = sch.Focus,
                    Scheduled = sch.StartDate,
                    EndDate = sch.EndDate,
                    MeetingUrl = sch.MeetingLink,
                    CoachAttended = sch.CoachAttended,
                    TeacherAttended = sch.TeacherAttended
                    
                };
            res.Data = respose;
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
