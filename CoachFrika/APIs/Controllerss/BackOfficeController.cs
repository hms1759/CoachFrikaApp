using CoachFrika.APIs.Domin.IServices;
using CoachFrika.APIs.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoachFrika.APIs.Controllerss
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class BackOfficeController : ControllerBase
    {
        private readonly ITeacherService _teacherService;
        private readonly ICoachesService _coachesService;
        private readonly IAccountService _accountService;
        public BackOfficeController(ITeacherService teacherService, ICoachesService coachesService, IAccountService accountService)
        {
            _teacherService = teacherService;
            _coachesService = coachesService;
            _accountService = accountService;
        }

        [HttpGet("GetAllApplicant")]
        public async Task<IActionResult> GetAllTeachers([FromQuery] GetTeachersSearch model)
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
        /// <param name="applicantId"></param>
        /// <returns></returns>
        [HttpPost("ApproveApplicantion/{applicantId}")]
        public async Task<IActionResult> ApproveApplicantion(string applicantId)
        {
            var result = await _accountService.ApproveApplicantion(applicantId);
            if (result.Status) return Ok(result); return BadRequest(result);
        }
    }
}
