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
using DnsClient;
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

    public class StudentsService : IStudentsService
    {
        private readonly AppDbContext _context;
        public readonly IEmailService _emailService;
        public readonly IWebHelpers _webHelpers;
        public readonly IAccountService _accountService;
        private readonly UserManager<CoachFrikaUsers> _userManager;
        private readonly UiSiteConfigSettings _uiSite;
        public StudentsService(IUnitOfWork unitOfWork,
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

        public async Task<BaseResponse<string>> CreateStudents(CreateStudentsDto model)
        {
            var tchId = _webHelpers.CurrentUserId();
            var res = new BaseResponse<string>();
            res.Status = true;
            var std = await _context.Students.FirstOrDefaultAsync(x => x.ClassId == model.ClassId && x.StudentNumber == model.ClassNumber);

            if (std != null)
            {
                res.Message = "Student already Onboarded";
                res.Status = false;
                return res;
            }

            var newEnt = new Students()
            {
                ClassId = model.ClassId,
                Name = model.Name,
                Class = model.Class,
                ParentName = model.ParentName,
                ParentPhoneNumber = model.ParentPhoneNumber,
                Address = model.Address,
                TeachersId = tchId,
                StudentNumber = model.ClassNumber,
            };
            var subjectList = _context.Subjects.Where(x => x.TeachersId == tchId);
                var listsheet = new List<StudentScoreSheet>();
            if (subjectList.Any())
            {
                foreach (var sub in subjectList)
                {
                    var scoreSheet = new StudentScoreSheet()
                    {
                        TeachersId = sub.TeachersId,
                        StudentId = newEnt.Id.ToString(),
                        SubjectId = sub.Id
                    }; 
                    listsheet.Add(scoreSheet);
                }
            }
            await _context.StudentScoreSheet.AddRangeAsync(listsheet);
            await _context.Students.AddAsync(newEnt);
            _context.SaveChanges();
            res.Message = "Student Successfully Onboarded";
            res.Status = true;
            return res;
        }

        public BaseResponse<List<Students>> GetAllStudents(GetStudentsSearch query)
        {
            var userId = _webHelpers.CurrentUserId();
            var res = new BaseResponse<List<Students>>();
            res.Status = true;
            try
            {
                // Apply filters based on the query parameters
                var cos = from rec in _context.Students
                          where (query.Name == null || rec.Name.Contains(query.Name))
                          && (query.ClassId == null || rec.ClassId == query.ClassId)
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

        public BaseResponse<List<Students>> GetAllStudentsScores(GetStudentsSearch query)
        {
            throw new NotImplementedException();
        }
    }
}
