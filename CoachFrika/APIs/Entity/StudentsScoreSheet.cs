using CoachFrika.Common.Enum;
using coachfrikaaaa.Common;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace coachfrikaaaa.APIs.Entity
{
    public class StudentScoreSheet : BaseEntity
    {
        public string? StudentId { get; set; }
        public string? TeachersId { get; set; }
        public int? FirstCA { get; set; }
        public int? SecondCA { get; set; }
        public int? Exam { get; set; }
        public string? SubjectId { get; set; }
    }
}
