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
        public Subscriptions? Plans { get; set; }

    }

    public class ResponseSchemesDto
    {
        public Guid Id { get; set; }
        public string? Plan { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? WeekNumber { get; set; }
        public string? CreatedBy { get; set; }

    }


    public class EditSchemesDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? WeekNumber { get; set; }
        public string? CreatedBy { get; set; }
        public Subscriptions? Plan { get; set; }

    }

}
