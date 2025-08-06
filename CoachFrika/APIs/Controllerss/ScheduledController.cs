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
            if(result.Status)return Ok(result);return BadRequest(result);
        }
        [HttpGet("GetCoachSchedule")]
        public IActionResult GetSchedule([FromQuery]GetSchedules query)
        {
            var result = _service.GetCoachSchedule(query);
            if(result.Status)return Ok(result);return BadRequest(result);
        }
        [HttpGet("GetScheduleList")]
        public IActionResult GetScheduleList()
        {
            var result = _service.GetScheduleList();
            if(result.Status)return Ok(result);return BadRequest(result);
        }
        [HttpGet("GetTeacherList")]
        public IActionResult GetTeacherList(string ScheduleId)
        {
            var result = _service.GetTeacherList(ScheduleId);
            if(result.Status)return Ok(result);return BadRequest(result);
        }

        [HttpPut("EditSchedule")]
        public IActionResult EditSchedule([FromBody] EditScheduleDto query)
        {
            var result = _service.EditSchedule(query);
            if(result.Status)return Ok(result);return BadRequest(result);
        }

        [HttpPut("AttendSchedle")]
        public IActionResult AttendSchedle([FromQuery] Guid Id)
        {
            var result = _service.AttendSchedle(Id);
            if(result.Status)return Ok(result);return BadRequest(result);
        }

        [HttpGet("GetScheduleById")]
        public IActionResult GetScheduleById([FromQuery] Guid Id)
        {
            var result = _service.GetScheduleById(Id);
            if(result.Status)return Ok(result);return BadRequest(result);
        }
    }
}