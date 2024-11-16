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

        public async Task<string> CreateCourse(CreateCoursesDto model)
        {
            try
            {
                var dto = new Courses();
                dto.CourseTitle = model.CourseTitle;
                dto.CourseIntro = model.CourseIntro;
                var schRepository = _unitOfWork.GetRepository<Courses>();
                await schRepository.AddAsync(dto);
                await _unitOfWork.SaveChangesAsync();
                return "Successful";
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);

            }
        }

        public async Task<string> CreateBatches(BatchesDto model)
        {
            try
            {
                var dto = new Batches();
                dto.Title = model.Title;
                var schRepository = _unitOfWork.GetRepository<Batches>();
                await schRepository.AddAsync(dto);
                await _unitOfWork.SaveChangesAsync();
                return "Successful";
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);

            }
        }

        public List<CoursesViewModel> GetCourses()
        {
            try
            {
                var cos = from course in _context.Courses.AsNoTracking()
                          select new CoursesViewModel
                          {
                              Id = course.Id,
                              CourseTitle = course.CourseTitle
                          };
                return cos.ToList();
            }
            catch (Exception ex)
            {
            throw new NotImplementedException(ex.Message);

            }

        }

        public List<BatchesViewModel> GetBatches()
        {
            try
            {
                var bat = from batche in _context.Batches.AsNoTracking()
                          select new BatchesViewModel
                          {
                              Id = batche.Id,
                              Title = batche.Title
                          };
                return bat.ToList();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);

            }
        }

        public async Task<string> CreateSchedule(SchedulesDto model)
        {
            try
            {

                var BatchRepository = _unitOfWork.GetRepository<Batches>();
                var checkBatch = await BatchRepository.GetByIdAsync(model.BatcheId);
                if (checkBatch == null)
                {
                    throw new NotImplementedException($"Batch with {model.BatcheId} not found");

                }
                var courseRepository = _unitOfWork.GetRepository<Batches>();
                var courseBatch = await courseRepository.GetByIdAsync(model.CourseId);
                if (courseBatch == null)
                {
                    throw new NotImplementedException($"Course with {model.CourseId} not found");

                }
                var dto = new Schedules();
                dto.Description = model.Description;
                dto.Subject = model.Subject;
                dto.MaterialUrl= model.MaterialUrl;
                dto.Schedule = model.Schedule;
                dto.BatcheId = model.BatcheId;
                dto.CourseId = model.CourseId;
                var schRepository = _unitOfWork.GetRepository<Schedules>();
                await schRepository.AddAsync(dto);
                await _unitOfWork.SaveChangesAsync();
                return "Successful";
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);

            }
        }
        public List<Schedules> GetMySchedule()
        {
            try
            { var user = _webHelpers.CurrentUser();
                var bat = from Schedule in _context.Schedules
                          where Schedule.CreatedBy == user
                          select Schedule;
                return bat.ToList();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);

            }
        }
    }
}
