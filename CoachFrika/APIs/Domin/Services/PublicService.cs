using CoachFrika.APIs.Domin.IServices;
using CoachFrika.APIs.ViewModel;
using CoachFrika.Common;
using CoachFrika.Common.AppUser;
using CoachFrika.Common.Extension;
using CoachFrika.Extensions;
using CoachFrika.Models;
using CoachFrika.Services;
using coachfrikaaaa.APIs.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;
using static CoachFrika.Common.LogingHandler.JwtServiceHandler;

namespace CoachFrika.APIs.Domin.Services
{
    public class PublicService : IPublicService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PublicService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<PublicCountDto> GetPublicCount()
        {
            

            throw new NotImplementedException();
        }
    }
}
