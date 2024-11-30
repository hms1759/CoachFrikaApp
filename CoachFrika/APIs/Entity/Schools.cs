using CoachFrika.Common.Enum;
using coachfrikaaaa.Common;
using Microsoft.AspNetCore.Identity;

namespace coachfrikaaaa.APIs.Entity
{
    public class SchoolEnrollmentRequest : BaseEntity
    {
        public string? SchoolName { get; set; }
        public string? SchoolAddress { get; set; }
        public string? SchoolPhoneNumber { get; set; }
        public string? SchoolEmail { get; set; }
        public string? HeadTeacherPhoneNumber { get; set; }
        public string? HeadTeacherEmail { get; set; }
        public string? HeadTeacherName { get; set; }
        public int NumbersOfTeachers { get; set; }
        public bool isSubscribed { get; set; }
        public Programtype? Programtype { get; set; }
        public string? Goals { get; set; }
        public string? ContactPersonEmail { get; set; }
        public string? ContactPersonName { get; set; }
        public string? ContactPersonPhoneNumber { get; set; }
    }
}
