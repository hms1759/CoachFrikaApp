﻿using CoachFrika.APIs.Domin.IServices;
using CoachFrika.APIs.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoachFrika.APIs.Controllerss
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class PublicController : ControllerBase
    {
        private readonly ILogicService _publicService;
        public PublicController(ILogicService publicService)
        {
            _publicService = publicService;
        }
        [HttpGet("GetLandingPageCount")]
        public async Task<IActionResult> GetPublicCount()
        {
            var result = await _publicService.GetPublicCount();
            return Ok(result);
        }
        [HttpPost("NewSubscription")]
        public async Task<IActionResult> NewSubscription([FromBody] SubscriptionDto model)
        {
            var result = await _publicService.NewSubscription(model);
            return Ok(result);
        }

        [HttpPost("ContactUs")]
        public async Task<IActionResult> ContactUs([FromBody] ContactUsDto model)
        {
            var logoUrl = $"{Request.Scheme}://{Request.Host}/images/logo.png";
            model.logoUrl = logoUrl;
            var result = await _publicService.ContactUs(model);
            return Ok(result);
        }
        [HttpPost("SchoolEnrollment")]
        public async Task<IActionResult> SchoolEnroll([FromBody] SchoolEnrollmentDto model)
        {
            var logoUrl = $"{Request.Scheme}://{Request.Host}/images/logo.png";
            model.logoUrl = logoUrl;
            var result = await _publicService.SchoolEnrollment(model);
            return Ok(result);
        }

        [HttpPost("CorsChecker")]
        public async Task<IActionResult> CorsChecker([FromBody] SchoolEnrollmentDto model)
        {
            var logoUrl = $"{Request.Scheme}://{Request.Host}/images/logo.png";
            model.logoUrl = logoUrl;
            var result = await _publicService.SchoolEnrollment(model);
            return Ok(result);
        }

        [HttpPost("SponsorAChild")]
        public async Task<IActionResult> SponsorAChild([FromBody] SponsorDto model)
        {
            var logoUrl = $"{Request.Scheme}://{Request.Host}/images/logo.png";
            model.logoUrl = logoUrl;
            var result = await _publicService.SponsorAchild(model);
            return Ok(result);
        }
    }
}
