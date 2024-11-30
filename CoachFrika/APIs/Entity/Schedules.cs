using coachfrikaaaa.Common;
using Microsoft.AspNetCore.Identity;

namespace coachfrikaaaa.APIs.Entity
{
    public class Schedules : BaseEntity
    {
        public string? Description { get; set; }
        public string? Subject { get; set; }
        public string? MaterialUrl { get; set; }
        public DateTime? Schedule { get; set; }
        //public Guid? BatcheId { get; set; }
        //public Batches? Batche { get; set; }
    }
}
