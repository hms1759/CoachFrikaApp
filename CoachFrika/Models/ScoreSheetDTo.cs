using coachfrikaaaa.APIs.Entity;
using coachfrikaaaa.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoachFrika.Models
{
    public class ScoreSheetDTo
    {
        public string? StudentId { get; set; }
        public string? SubjectId { get; set; }
        public string? StudentName { get; set; }
        public string? ParentNumber { get; set; }
        public string? ClassName { get; set; }
        public string? Subject { get; set; }
        public int? FirstCA { get; set; }
        public int? SecondCA { get; set; }
        public int? Exam { get; set; }
        public int? Total { get; set; }
    }
    public class ResponseScoreSheetDTo
    {
        public bool IsAll { get; set; }
        public List<ScoreSheetDTo>? score { get; set; }
    }


}
