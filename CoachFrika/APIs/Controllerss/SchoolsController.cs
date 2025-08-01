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
    // [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Coach},{AppRoles.SuperAdmin}")]
    [AllowAnonymous]
    [ApiController]
    public class SchoolsController : BaseController
    {
        private readonly ISchoolsService _service;
        private readonly ILogicService _logicService;
        public SchoolsController(ISchoolsService service, ILogicService logicService)
        {
            _service = service;
            _logicService = logicService;
        }
        [HttpPost("CreateSchool")]
        public async Task<IActionResult> CreateSchedule(CreateSchoolDto model)
        {
            var result = await _service.CreateSchools(model);
            if (result.Status) return Ok(result); return BadRequest(result);
        }
        [HttpGet("GetSchools")]
        public IActionResult GetSchedule([FromQuery] GetSchoolSearch query)
        {
            var result = _service.GetAllSchools(query);
            if(result.Status)return Ok(result);return BadRequest(result);
        }
        //Get All teacher in the School by schoolId
        [HttpGet("GetSchoolById/{Id}")]
        public async Task<IActionResult> GetSchoolById(Guid Id)
        {
            var result = await _service.GetSchoolById(Id);
            if (result.Status) return Ok(result); return BadRequest(result);
        }
        [HttpGet("GetSchoolTeachers")]
        public IActionResult GetSchoolTeachers([FromQuery] GetSchoolTeachersSearch query)
        {
            var result = _service.GetSchoolTeachers(query);
            if (result.Status) return Ok(result); return BadRequest(result);
        }
        [HttpGet("InviteSchoolTeacher")]
        public async Task<IActionResult> InviteSchoolTeacher(CreateSchoolTeacherDto model)
        {
            var result =await  _service.InviteSchoolTeacher(model);
            if(result.Status)return Ok(result);return BadRequest(result);
        }
       
    }
}