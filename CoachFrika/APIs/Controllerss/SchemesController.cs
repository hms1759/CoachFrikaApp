using CoachFrika.APIs.Domin.IServices;
using CoachFrika.APIs.ViewModel;
using CoachFrika.Common;
using CoachFrika.Common.AppUser;
using CoachFrika.Common.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoachFrika.APIs.Controllerss
{
    [Route("api/[controller]")]
     [Authorize]///(Roles = $"{AppRoles.Admin},{AppRoles.Coach},{AppRoles.SuperAdmin}")]

    [ApiController]
    public class SchemesController : BaseController
    {
        private readonly IStudentsService _service;
        private readonly ILogicService _logicService;
        public SchemesController(IStudentsService service, ILogicService logicService)
        {
            _service = service;
            _logicService = logicService;
        }
        [HttpPost("CreateScheme")]
        public async Task<IActionResult> CreateScheme(CreateSchemesDto model)
        {
            var result = await _logicService.CreateScheme(model);
            if (result.Status) return Ok(result); return BadRequest(result);
        }
        [HttpGet("GetAllScheme")]
        public IActionResult GetAllScheme([FromQuery] GetSchemeSearch query)
        {
            var result = _logicService.GetAllScheme(query);
            if (result.Status) return Ok(result); return BadRequest(result);
        }
        [HttpGet("GetAllSchemeByPlan")]
        public IActionResult GetAllSchemeByPlan([FromQuery]Subscriptions query)
        {
            var result = _logicService.GetAllSchemeByPlan(query);
            if (result.Status) return Ok(result); return BadRequest(result);
        }


    }
}