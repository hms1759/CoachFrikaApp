using coachfrikaaaa.Common;
using Microsoft.AspNetCore.Identity;

namespace coachfrikaaaa.APIs.Entity
{
    public class NewsSubscription : BaseEntity
    {
        public string? Email { get; set; }
    }
}
