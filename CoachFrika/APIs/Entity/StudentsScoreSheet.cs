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
        public string? FirstCA { get; set; }
        public string? SecondCA { get; set; }
        public string? Exam { get; set; }
        [ForeignKey(nameof(Subject))]
        public Guid SubjectId { get; set; }
        public virtual Subjects? Subject { get; set; }
    }
}
