using coachfrikaaaa.Common;
using Microsoft.AspNetCore.Identity;

namespace coachfrikaaaa.APIs.Entity
{
    public class Subjects : BaseEntity
    {
        public string? SubjectName { get; set; }
        public string? SubjectCode { get; set; }
        public string? TeachersId { get; set; }
    }
}
