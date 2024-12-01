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
    [Authorize]
    [ApiController]
    public class TeachersController : BaseController
    {
        private readonly ILogicService _service;
        public TeachersController(ILogicService service)
        {
            _service = service;
        }

        [HttpGet("GetSchool")]
        public IActionResult GetSchool()
        { 
              var result =  _service.GetSchool();
            return Ok(result);
        }
        [HttpGet("GetSubject")]
        public IActionResult GetSubject()
        {
            var result =  _service.GetSubject();
            return Ok(result);
        }
        //[HttpPost("CreateSchool")]
        //public async Task<IActionResult> CreateSchool(string school)
        //{
        //    var result = await _service.CreateSchool(school);
        //    return Ok(result);
        //}
        [HttpPost("CreateSubject")]
        public async Task<IActionResult> CreateSubject(List<string> school)
        {
            var result = await _service.CreateSubject(school);
            return Ok(result);
        }

        [HttpGet("GetTeacherById")]
        public async Task<IActionResult> GetTeacherById([FromQuery] Guid userId)
        {
            var result = await _service.GetUserById(userId);
            return Ok(result);
        }
        [HttpGet("GetTeacherDetails")]
        public async Task<IActionResult> GetTeacherDetails()
        {
            var result = await _service.GetUserDetails();
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