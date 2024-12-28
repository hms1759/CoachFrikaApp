using CoachFrika.APIs.Domin.IServices;
using CoachFrika.APIs.ViewModel;
using CoachFrika.Common;
using CoachFrika.Common.AppUser;
using coachfrikaaaa.APIs.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoachFrika.APIs.Controllerss
{
    [Route("api/[controller]")]
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


        [HttpGet("MyTeachers")]
        public IActionResult MyTeachers([FromQuery]GetTeachers model)
        {
            var result =  _coachesService.MyTeachers(model);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("GetAllCoaches")]
        public async Task<IActionResult> GetAllCoaches([FromQuery] GetAllCoaches model)
        {
            var result =  _coachesService.GetAllCoaches(model);
            return Ok(result);
        }

        [HttpGet("GetCoacheById")]
        public async Task<IActionResult> GetCoacheById([FromQuery] string Id)
        {
            var result = await _coachesService.GetCoachById(Id);
            return Ok(result);
        }

        [HttpPost("AddTeachersRecomendatins")]
        public async Task<IActionResult> AddRecomendatins(CoachRecommendation model)
        {
            var result = await _coachesService.AddRecomendations(model);
            return Ok(result);
        }

        [HttpPut("EditRecommendation")]
        public async Task<IActionResult> EditRecommendation(EditRecommendation model)
        {
            var result = await _coachesService.EditRecommendation(model);
            return Ok(result);
        }

        [HttpGet("GetAllRecommendations")]
        public async Task<IActionResult> GetAllRecommendations([FromQuery] GetCoachesRecommendations model)
        {
            var result = _coachesService.Recommendations(model);
            return Ok(result);
        }


        [HttpGet("GetRecommendationById")]
        public async Task<IActionResult> GetRecommendationById([FromQuery] string Id)
        {
            var result = await _coachesService.GetRecommendationById(Id);
            return Ok(result);
        }
    }
}