using CoachFrika.Common.Enum;
using coachfrikaaaa.Common;

namespace CoachFrika.APIs.Entity
{
    public class Payment :BaseEntity
    {
        public int Amount { get; set; }
        public Subscriptions Subscription { get; set; }
        public string Paymentrefernce { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}
