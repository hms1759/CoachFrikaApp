using CoachFrika.APIs.Domin.IServices;
using CoachFrika.APIs.ViewModel;
using CoachFrika.Common;
using CoachFrika.Common.AppUser;
using coachfrikaaaa.APIs.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("CreateStage2")]
        public async Task<IActionResult> CreateStage2(PhoneYearsDto model)
        {
            var result = await _coachesService.CreateStage2(model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("CreateStage3")]
        public async Task<IActionResult> CreateStage3(DescriptionDto model)
        {
            var result = await _coachesService.CreateStage3(model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("CreateStage4")]
        public async Task<IActionResult> CreateStage4(SocialMediaDto model)
        {
            var result = await _coachesService.CreateStage4(model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        //[HttpPost("CreateStage5")]
        //public async Task<IActionResult> CreateStage5(SubscriptionsDto model)
        //{
        //    var result = await _coachesService.CreateStage5(model);
        //    if (result.Status)
        //    {
        //        return Ok(result);
        //    }
        //    return BadRequest(result);
        //}


        [HttpGet("MyTeachers")]
        public IActionResult MyTeachers([FromQuery]GetTeachers model)
        {
            var result =  _coachesService.MyTeachers(model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetAllCoaches")]
        public async Task<IActionResult> GetAllCoaches([FromQuery] GetAllCoaches model)
        {
            var result =  _coachesService.GetAllCoaches(model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetCoacheById")]
        public async Task<IActionResult> GetCoacheById([FromQuery] string Id)
        {
            var result = await _coachesService.GetCoachById(Id);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("AddTeachersRecomendations")]
        public async Task<IActionResult> AddRecomendatins(CoachRecommendation model)
        {
            var result = await _coachesService.AddRecomendations(model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("EditRecommendation")]
        public async Task<IActionResult> EditRecommendation(EditRecommendation model)
        {
            var result = await _coachesService.EditRecommendation(model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [ProducesResponseType(typeof(BaseResponse<List<GetCoachesRecommendationResponse>>), (int)HttpStatusCode.OK)]
        [HttpGet("GetAllRecommendations")]
        public async Task<IActionResult> GetAllRecommendations([FromQuery] GetCoachesRecommendations model)
        {
            var result = _coachesService.Recommendations(model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpGet("GetRecommendationById")]
        public async Task<IActionResult> GetRecommendationById([FromQuery] string Id)
        {
            var result = await _coachesService.GetRecommendationById(Id);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpPost("AddRecomendationsComment")]
        public async Task<IActionResult> AddRecomendationsComment(CoachRecommendationComment model)
        {
            var result = await _coachesService.AddRecomendationsComment(model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetRecommendationTeacherList")]
        public IActionResult GetRecommendationTeacherList([FromQuery] GetRecomendationTeachers Recomendation)
        {
            var result = _coachesService.GetRecommendationTeacherList(Recomendation);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}