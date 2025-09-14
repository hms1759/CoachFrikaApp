using CoachFrika.Common.Enum;
using coachfrikaaaa.Common;
using Microsoft.AspNetCore.Identity;

namespace coachfrikaaaa.APIs.Entity
{
    public class Subjects : BaseEntity
    {
        public SchoolSubjectEnum? SubjectId { get; set; }
        public string? Subject { get; set; }
        public string? TeachersId { get; set; }
    }
}
