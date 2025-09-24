using CoachFrika.APIs.Domin.IServices;
using CoachFrika.APIs.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoachFrika.APIs.Controllerss
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackOfficeAPIController : ControllerBase
    {
        private readonly ITeacherService _teacherService;
        private readonly ICoachesService _coachesService;
        private readonly ISchoolsService _schoolService;
        private readonly IAccountService _accountService;
        private readonly ICousesService _cousesService;
        private readonly ILogicService _publicService;
        public BackOfficeAPIController(ITeacherService teacherService, ISchoolsService schoolService, ICoachesService coachesService, IAccountService accountService, ICousesService cousesService, ILogicService publicService)
        {
            _teacherService = teacherService;
            _coachesService = coachesService;
            _accountService = accountService;
            _schoolService = schoolService;
            _cousesService = cousesService;
            _publicService = publicService;
        }
        [HttpGet("GetDashBoardPageCount")]
        public async Task<IActionResult> GetDashBoardPageCount()
        {
            var result = await _publicService.GetPublicCount();
            if (result.Status)
            {
                if (result.Status) return Ok(result); return BadRequest(result);
            }
            return BadRequest(result);
        }
        [HttpGet("GetAllApplicant")]
        public async Task<IActionResult> GetAllApplicant([FromQuery] GetTeachersSearch model)
        {
            var result = model.IsCoach ? _coachesService.GetAllCoaches(model) : _teacherService.GetTeachers(model);
            if (result.Status) return Ok(result); return BadRequest(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetApplicantDetails/{Id}")]
        public async Task<IActionResult> GetApplicantDetails(string Id)
        {
            var result = await _accountService.GetApplicant(Id);
            if (result.Status) return Ok(result); return BadRequest(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPut("ApproveApplication/{Id}")]
        public async Task<IActionResult> ApproveApplicantion(string Id)
        {
            var result = await _accountService.ApproveApplicantion(Id);
            if (result.Status) return Ok(result); return BadRequest(result);
        }

        [HttpGet("GetAllSchoolApplicant")]
        public IActionResult GetAllSchoolApplicant([FromQuery] GetSchoolSearch model)
        {
            var result =  _schoolService.GetAllSchools(model);
            if (result.Status) return Ok(result); return BadRequest(result);
        }

        [HttpGet("GetAllSchedules")]
        public IActionResult GetMySchedule([FromQuery] GetBackOfficeScheduleSearch model)
        {
            var result = _cousesService.GetBackOfficeScheduleList(model);
            if (result.Status) return Ok(result); return BadRequest(result);
        }

    }
    
}
