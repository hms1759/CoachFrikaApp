using CoachFrika.Common.Enum;
using coachfrikaaaa.APIs.Entity;
using coachfrikaaaa.Common;

namespace CoachFrika.APIs.Entity
{
    public class Payment :BaseEntity
    {
        public int Amount { get; set; }
        public Subscriptions Subscription { get; set; }
        public string? Paymentrefernce { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string? UserId { get; set; }
        public virtual CoachFrikaUsers? User { get; set; }
    }
}
