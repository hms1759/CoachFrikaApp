using System.ComponentModel.DataAnnotations;
namespace CoachFrika.Models
{
    public class ProfileSetupViewModel
    {
        public string Title { get; set; }
        public string Background { get; set; }
        public string SchoolName { get; set; }
        public string SchoolAddress { get; set; }
        public int? StudentsCount { get; set; }
        public int? YearsExperience { get; set; }
        public string SubjectTaught { get; set; }
        public IFormFile ProfilePicture { get; set; }
    }
}
