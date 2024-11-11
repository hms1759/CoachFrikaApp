using coachfrikaaaa.Common;

namespace coachfrikaaaa.APIs.Entity
{
    public class ContactUs : BaseEntity
    {
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Message { get; set; }
    }
}
