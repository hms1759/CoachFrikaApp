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
     [Authorize]///(Roles = $"{AppRoles.Admin},{AppRoles.Coach},{AppRoles.SuperAdmin}")]

    [ApiController]
    public class StudentsController : BaseController
    {
        private readonly IStudentsService _service;
        private readonly ILogicService _logicService;
        public StudentsController(IStudentsService service, ILogicService logicService)
        {
            _service = service;
            _logicService = logicService;
        }
        [HttpPost("CreateStudent")]
        public async Task<IActionResult> CreateSchedule(CreateStudentsDto model)
        {
            var result = await _service.CreateStudents(model);
            if (result.Status) return Ok(result); return BadRequest(result);
        }
        [HttpGet("GetAllStudents")]
        public IActionResult GetAllStudents([FromQuery] GetStudentsSearch query)
        {
            var result = _service.GetAllStudents(query);
            if (result.Status) return Ok(result); return BadRequest(result);
        }
        [HttpGet("GetAllStudentsScores")]
        public IActionResult GetAllStudentsScores([FromQuery] ScoreSheetsSearch query)
        {
            var result = _service.GetAllStudentsScores(query);
            if (result.Status) return Ok(result); return BadRequest(result);
        }

        [HttpPost("UpdateScores")]
        public async Task<IActionResult> CreateStudentScores(List<StudentScoreDto> model)
        {
            var result = await _service.CreateStudentsScores(model);
            if (result.Status) return Ok(result); return BadRequest(result);
        }
        ////Get All teacher in the Student by StudentId
        //[HttpGet("GetStudentById/{Id}")]
        //public async Task<IActionResult> GetStudentById(Guid Id)
        //{
        //    var result = await _service.GetStudentById(Id);
        //    if (result.Status) return Ok(result); return BadRequest(result);
        //}
        //[HttpGet("GetStudents")]
        //public IActionResult GetStudentTeachers([FromQuery] GetStudentsSearch query)
        //{
        //    var result = _service.GetStudents(query);
        //    if (result.Status) return Ok(result); return BadRequest(result);
        //}


    }
}