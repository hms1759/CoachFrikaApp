using AutoMapper;
using CoachFrika.APIs.ViewModel;
using coachfrikaaaa.APIs.Entity;

namespace CoachFrika.Common.AutoMapper
{
    public class AutoMapperClass :Profile
    {
        public AutoMapperClass()
        {
            CreateMap<CoachFrikaUsers, ProfileDto>();
        }
    }
}
