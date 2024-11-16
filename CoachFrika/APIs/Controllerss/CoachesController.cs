using CoachFrika.APIs.Domin.IServices;
using CoachFrika.APIs.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoachFrika.APIs.Controllerss
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoachesController : ControllerBase
    {
        private readonly ICousesService _service;
        private readonly ILogicService _logicService;
        public CoachesController(ICousesService service, ILogicService logicService)
        {
            _service = service;
            _logicService = logicService;
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
        [HttpGet("GetBatches")]
        public IActionResult GetMySchedule()
        {
            var result = _service.GetMySchedule();
            return Ok(result);
        }
    }
}