using CoachFrika.APIs.Domin.IServices;
using CoachFrika.APIs.Domin.Services;
using CoachFrika.APIs.ViewModel;
using CoachFrika.Common;
using CoachFrika.Common.AppUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CoachFrika.APIs.Controllerss
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Teacher")]
    [ApiController]
    public class TeachersController : BaseController
    {
        private readonly ILogicService _service;
        private readonly ITeacherService _teacherService;
        public TeachersController(ILogicService service, ITeacherService teacherService)
        {
            _service = service;
            _teacherService = teacherService;
        }

        [HttpPost("CreateStage1")]
        public async Task<IActionResult> CreateStage1(TitleDto model)
        {
            var result = await _teacherService.CreateStage1(model);
            if(result.Status)return Ok(result);return BadRequest(result);
        }
        [HttpPost("CreateStage2")]
        public async Task<IActionResult> CreateStage2(TeacherPhoneYearsDto model)
        {
            var result = await _teacherService.CreateStage2(model);
            if(result.Status)return Ok(result);return BadRequest(result);
        }
        [HttpPost("CreateStage3")]
        public async Task<IActionResult> CreateStage3(DescriptionDto model)
        {
            var result = await _teacherService.CreateStage3(model);
            if(result.Status)return Ok(result);return BadRequest(result);
        }
        [HttpPost("CreateStage4")]
        public async Task<IActionResult> CreateStage4(SocialMediaDto model)
        {
            var result = await _teacherService.CreateStage4(model);
            if(result.Status)return Ok(result);return BadRequest(result);
        }
        [HttpPost("CreateStage5")]
        public async Task<IActionResult> CreateStage5(SchoolesdescriptionDto model)
        {
            var result = await _teacherService.CreateStage5(model);
            if(result.Status)if(result.Status)return Ok(result);return BadRequest(result);return BadRequest(result);
        }
        [HttpPost("CreateStage6")]
        public async Task<IActionResult> CreateStage6(SubscriptionsDto model)
        {
            var logoUrl = $"{Request.Scheme}://{Request.Host}/images/logo.png";
            model.Logo = logoUrl;
            var result = await _teacherService.CreateStage6(model);
            if(result.Status)return Ok(result);return BadRequest(result);
        }
        [HttpGet("GetMySchedule")]
        public IActionResult GetMySchedule([FromQuery]GetSchedules query)
        {
            var result = _teacherService.GetMySchedule(query);
            if(result.Status)return Ok(result);return BadRequest(result);
        }

        [HttpPut("SelectCoach")]
        public async Task<IActionResult> SelectCoach([FromQuery] Guid CoachId)
        {
            var result = await _teacherService.SelectCoach(CoachId);
            if(result.Status)return Ok(result);return BadRequest(result);
        }
        [HttpGet("GetTeacherById")]
        public async Task<IActionResult> GetTeacherById([FromQuery] string Id)
        {
            var result = await _teacherService.GetTeacherById(Id);
            if(result.Status)return Ok(result);return BadRequest(result);
        }

        [ProducesResponseType(typeof(BaseResponse<List<GetTeacherRecommendationResponse>>), (int)HttpStatusCode.OK)]
        [HttpGet("GetAllRecommendations")]
        public async Task<IActionResult> GetAllRecommendations([FromQuery] GetTeacherRecommendations model)
        {
            var result = _teacherService.Recommendations(model);
            if(result.Status)return Ok(result);return BadRequest(result);
        }

        [HttpPut("RecommendationRemark")]
        public async Task<IActionResult> RecommendationRemark(TeachersRemark model)
        {
            var result = await _teacherService.RecommendationRemark(model);
            if(result.Status)return Ok(result);return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPost("backStage")]
        public async Task<IActionResult> backStage(string email)
        {
            var result = await _teacherService.backStage(email);
            if(result.Status)return Ok(result);return BadRequest(result);
        }
        //[HttpGet("GetSubject")]
        //public IActionResult GetSubject()
        //{
        //    var result =  _service.GetSubject();
        //    if(result.Status)return Ok(result);return BadRequest(result);
        //}
        ////[HttpPost("CreateSchool")]
        ////public async Task<IActionResult> CreateSchool(string school)
        ////{
        ////    var result = await _service.CreateSchool(school);
        ////    if(result.Status)return Ok(result);return BadRequest(result);
        ////}
        //[HttpPost("CreateSubject")]
        //public async Task<IActionResult> CreateSubject(List<string> school)
        //{
        //    var result = await _service.CreateSubject(school);
        //    if(result.Status)return Ok(result);return BadRequest(result);
        //}

        //[HttpGet("GetTeacherById")]
        //public async Task<IActionResult> GetTeacherById([FromQuery] Guid userId)
        //{
        //    var result = await _service.GetUserById(userId);
        //    if(result.Status)return Ok(result);return BadRequest(result);
        //}
        //[HttpGet("GetTeacherDetails")]
        //public async Task<IActionResult> GetTeacherDetails()
        //{
        //    var result = await _service.GetUserDetails();
        //    if(result.Status)return Ok(result);return BadRequest(result);
        //}
        //[HttpGet("GetMySchedule")]
        //public IActionResult GetMySchedule()
        //{
        //    var result = _service.GetMySchedule();
        //    if(result.Status)return Ok(result);return BadRequest(result);
        //}
    }
}