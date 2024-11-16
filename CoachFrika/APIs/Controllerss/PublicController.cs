using CoachFrika.APIs.Domin.IServices;
using CoachFrika.APIs.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoachFrika.APIs.Controllerss
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicController : ControllerBase
    {
        private readonly IPublicService _publicService;
        public PublicController(IPublicService publicService)
        {
            _publicService = publicService;
        }
        [HttpPost("GetLandingPage")]
        public async Task<IActionResult> GetPublicCount()
        {
            var result = await _publicService.GetPublicCount();
            return Ok(result);
        }
    }
}
