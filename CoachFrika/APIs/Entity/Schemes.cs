using CoachFrika.Common.Enum;
using coachfrikaaaa.Common;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace coachfrikaaaa.APIs.Entity
{
    public class Schemes : BaseEntity
    {
        [Required]
        public Subscriptions? Plan { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public int? WeekNumber { get; set; }

    }
}
