using CoachFrika.APIs.Domin.IServices;
using CoachFrika.APIs.ViewModel;
using CoachFrika.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoachFrika.APIs.Controllerss
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ContactUsController : Controller
    {
        private readonly IContactUsService _contactUsService;
        public ContactUsController(IContactUsService contactUsService)
        {
            _contactUsService = contactUsService;
        }
        [AllowAnonymous]
        [HttpPost("CreateContactUs")]
        public async Task<IActionResult> ContactUs(ContactFormModel model)
        {
            var logoUrl = $"{Request.Scheme}://{Request.Host}/images/logo.png";
            var result = await _contactUsService.CreateContactUs(model, logoUrl);
           if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
