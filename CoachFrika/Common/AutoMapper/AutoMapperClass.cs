
using CoachFrika.APIs.ViewModel;
using coachfrikaaaa.APIs.Entity;

namespace CoachFrika.Common.AutoMapper
{
    public class ProfileMapper
    {
        // Method to map from CoachFrikaUsers to ProfileDto
        public static ProfileDto MapToProfileDto(CoachFrikaUsers coachFrikaUser)
        {
            if (coachFrikaUser == null) return null;

            return new ProfileDto
            {
                FullName = coachFrikaUser.FullName,
                TweeterUrl = coachFrikaUser.TweeterUrl,
                LinkedInUrl = coachFrikaUser.LinkedInUrl,
                InstagramUrl = coachFrikaUser.InstagramUrl,
                FacebookUrl = coachFrikaUser.FacebookUrl,
                Role = coachFrikaUser.Role,
                StateOfOrigin = coachFrikaUser.StateOfOrigin,
                ProfessionalTitle = coachFrikaUser.ProfessionalTitle,
                Nationality = coachFrikaUser.Nationality,
                Address = coachFrikaUser.Address,
                Title = coachFrikaUser.Title,
                Description = coachFrikaUser.Description,
                Subscriptions = coachFrikaUser.Subscriptions,
                NumbersOfStudents = coachFrikaUser.NumbersOfStudents,
                YearStartExperience = coachFrikaUser.YearStartExperience,
                Stages = coachFrikaUser.Stages,
                CoachId = coachFrikaUser.CoachId,
                TeacherId = coachFrikaUser.TeacherId
            };
        }

        // Method to map from ProfileDto to CoachFrikaUsers
        public static CoachFrikaUsers MapToCoachFrikaUser(ProfileDto profileDto)
        {
            if (profileDto == null) return null;

            return new CoachFrikaUsers
            {
                FullName = profileDto.FullName,
                TweeterUrl = profileDto.TweeterUrl,
                LinkedInUrl = profileDto.LinkedInUrl,
                InstagramUrl = profileDto.InstagramUrl,
                FacebookUrl = profileDto.FacebookUrl,
                Role = profileDto.Role,
                StateOfOrigin = profileDto.StateOfOrigin,
                ProfessionalTitle = profileDto.ProfessionalTitle,
                Nationality = profileDto.Nationality,
                Address = profileDto.Address,
                Title = profileDto.Title,
                Description = profileDto.Description,
                Subscriptions = profileDto.Subscriptions,
                NumbersOfStudents = profileDto.NumbersOfStudents,
                YearStartExperience = profileDto.YearStartExperience,
                Stages = profileDto.Stages,
                CoachId = profileDto.CoachId,
                TeacherId = profileDto.TeacherId
            };
        }
    }

}
