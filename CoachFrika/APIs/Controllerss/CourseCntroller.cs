using CoachFrika.APIs.Domin.IServices;
using CoachFrika.APIs.ViewModel;
using CoachFrika.Common;
using CoachFrika.Common.AppUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoachFrika.APIs.Controllerss
{
    [Route("api/[controller]")]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Coach},{AppRoles.SuperAdmin}")]
    [ApiController]
    public class CourseCntroller : BaseController
    {
        private readonly ICousesService _service;
        private readonly ILogicService _logicService;
        private readonly ICoachesService _coachesService;
        public CourseCntroller(ICousesService service, ILogicService logicService, ICoachesService coachesService)
        {
            _service = service;
            _logicService = logicService;
            _coachesService = coachesService;
        }
        [HttpPost("CreateCourse")]
        public async Task<IActionResult> CreateCourse(CreateCoursesDto model)
        {
            var result = await _service.CreateCourse(model);
            return Ok(result);
        }
        [HttpPost("CreateBatches")]
        public async Task<IActionResult> CreateBatches(BatchesDto model)
        {
            var result = await _service.CreateBatches(model);
            return Ok(result);
        }

        [HttpGet("GetCoachesById")]
        public async Task<IActionResult> GetCoachesById([FromQuery] Guid userId)
        {
            var result = await _logicService.GetUserById(userId);
            return Ok(result);
        }
        [HttpGet("GetCoachesDetails")]
        public async Task<IActionResult> GetCoachesDetails()
        {
            var result = await _logicService.GetUserDetails();
            return Ok(result);
        }
        [HttpGet("GetCourse")]
        public IActionResult GetCourse()
        {
            var result =  _service.GetCourses();
            return Ok(result);
        }
        [HttpGet("GetBatches")]
        public IActionResult GetBatches()
        {
            var result = _service.GetBatches();
            return Ok(result);
        }

        [HttpPost("CreateSchedule")]
        public async Task<IActionResult> CreateSchedule(SchedulesDto model)
        {
            var result = await _service.CreateSchedule(model);
            return Ok(result);
        }
        [HttpGet("GetMySchedule")]
        public IActionResult GetMySchedule()
        {
            var result = _service.GetMySchedule();
            return Ok(result);
        }
    }
}