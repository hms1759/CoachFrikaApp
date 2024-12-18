using coachfrikaaaa.Common;

namespace CoachFrika.APIs.Entity
{
    public class ChildSponsor : BaseEntity
    {
        public string? SponsorName { get; set; }
        public string? SponsorEmail { get; set; }
        public string? SponsorPhoneNumber { get; set; }
        public int NumbersOfChildren { get; set; }
    }
}
