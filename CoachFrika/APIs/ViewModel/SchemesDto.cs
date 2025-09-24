using CoachFrika.Common.Enum;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CoachFrika.APIs.ViewModel
{
    public class CreateSchemesDto
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

    public class GetSchemeSearch : Pagination
    {
        public string? Title { get; set; }
        public int? WeekNumber { get; set; }
        public Subscriptions? Plan { get; set; }

    }
  
}
