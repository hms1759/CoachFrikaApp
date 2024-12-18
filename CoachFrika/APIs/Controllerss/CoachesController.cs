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
    [AllowAnonymous]
    [ApiController]
    public class CoachesController : BaseController
    {
        private readonly ICoachesService _coachesService;
        public CoachesController(ICoachesService coachesService)
        {
            _coachesService = coachesService;
        }

        [HttpPost("CreateStage1")]
        public async Task<IActionResult> CreateStage1(TitleDto model)
        {
            var result = await _coachesService.CreateStage1(model);
            return Ok(result);
        }
        [HttpPost("CreateStage2")]
        public async Task<IActionResult> CreateStage2(PhoneYearsDto model)
        {
            var result = await _coachesService.CreateStage2(model);
            return Ok(result);
        }
        [HttpPost("CreateStage3")]
        public async Task<IActionResult> CreateStage3(DescriptionDto model)
        {
            var result = await _coachesService.CreateStage3(model);
            return Ok(result);
        }
        [HttpPost("CreateStage4")]
        public async Task<IActionResult> CreateStage4(SocialMediaDto model)
        {
            var result = await _coachesService.CreateStage4(model);
            return Ok(result);
        }
        [HttpPost("CreateStage5")]
        public async Task<IActionResult> CreateStage5(SubscriptionsDto model)
        {
            var result = await _coachesService.CreateStage5(model);
            return Ok(result);
        }


        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Coach},{AppRoles.SuperAdmin}")]
        [HttpGet("MyTeachers")]
        public IActionResult MyTeachers([FromQuery]GetTeachers model)
        {
            var result =  _coachesService.MyTeachers(model);
            return Ok(result);
        }
        [HttpPost("GetAllCoaches")]
        public async Task<IActionResult> GetAllCoaches(GetAllCoaches model)
        {
            var result =  _coachesService.GetAllCoaches(model);
            return Ok(result);
        }

        [HttpPost("GetCoacheById")]
        public async Task<IActionResult> GetCoacheById([FromQuery] string Id)
        {
            var result = await _coachesService.GetCoachById(Id);
            return Ok(result);
        }
    }
}