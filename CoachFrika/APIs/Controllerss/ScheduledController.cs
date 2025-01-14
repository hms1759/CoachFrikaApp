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
    public class ScheduledController : BaseController
    {
        private readonly ICousesService _service;
        private readonly ILogicService _logicService;
        public ScheduledController(ICousesService service, ILogicService logicService)
        {
            _service = service;
            _logicService = logicService;
        }
        [HttpPost("CreateSchedule")]
        public async Task<IActionResult> CreateSchedule(CreateScheduleDto model)
        {
            var result = await _service.CreateSchedule(model);
            return Ok(result);
        }
        [HttpGet("GetCoachSchedule")]
        public IActionResult GetSchedule([FromQuery]GetSchedules query)
        {
            var result = _service.GetCoachSchedule(query);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("GetScheduleList")]
        public IActionResult GetScheduleList()
        {
            var result = _service.GetScheduleList();
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("GetTeacherList")]
        public IActionResult GetTeacherList(string ScheduleId)
        {
            var result = _service.GetTeacherList(ScheduleId);
            return Ok(result);
        }



        [HttpPut("EditSchedule")]
        public IActionResult EditSchedule([FromBody] EditScheduleDto query)
        {
            var result = _service.EditSchedule(query);
            return Ok(result);
        }

        [HttpPut("AttendSchedle")]
        public IActionResult AttendSchedle([FromQuery] Guid Id)
        {
            var result = _service.AttendSchedle(Id);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetScheduleById")]
        public IActionResult GetScheduleById([FromQuery] Guid Id)
        {
            var result = _service.GetScheduleById(Id);
            return Ok(result);
        }



        //[HttpGet("GetCoachesById")]
        //public async Task<IActionResult> GetCoachesById([FromQuery] Guid userId)
        //{
        //    var result = await _logicService.GetUserById(userId);
        //    return Ok(result);
        //}
        //[HttpGet("GetCoachesDetails")]
        //public async Task<IActionResult> GetCoachesDetails()
        //{
        //    var result = await _logicService.GetUserDetails();
        //    return Ok(result);
        //}
        //[HttpGet("GetBatches")]
        //public IActionResult GetBatches()
        //{
        //    var result = _service.GetBatches();
        //    return Ok(result);
        //}

        //[HttpPost("CreateSchedule")]
        //public async Task<IActionResult> CreateSchedule(SchedulesDto model)
        //{
        //    var result = await _service.CreateSchedule(model);
        //    return Ok(result);
        //}
        //[HttpGet("GetMySchedule")]
        //public IActionResult GetMySchedule()
        //{
        //    var result = _service.GetMySchedule();
        //    return Ok(result);
        //}
    }
}