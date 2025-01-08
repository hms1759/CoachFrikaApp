using System.Text.Json.Serialization;

namespace CoachFrika.APIs.ViewModel
{
    public class WebHookAuthorizationWe
    {
        public string authorization_code { get; set; }
        public string bin { get; set; }
        public string last4 { get; set; }
        public string exp_month { get; set; }
        public string exp_year { get; set; }
        public string card_type { get; set; }
        public string channel { get; set; }
        public string bank { get; set; }
    }

    public class WebHookCustomer
    {
        public long id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }

    public class Data
    {
        public int id { get; set; }
        public string domain { get; set; }
        public string status { get; set; }
        public string reference { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public int paid_at { get; set; }
        public int created_at { get; set; }
        public int updated_at { get; set; }
        public string gateway_response { get; set; }
        public string payment_type { get; set; }
        public string channel { get; set; }
        public int paid_amount { get; set; }
        public int merchant_fee { get; set; }
        public WebHookCustomer customer { get; set; }
        public WebHookAuthorizationWe authorization { get; set; }
        public object plan { get; set; }
        public int amount_settled { get; set; }
        public object order_id { get; set; }
    }

    public class PaymentWebHook
    {
        public string @event { get; set; }
        public Data data { get; set; }
        [JsonIgnore]
        public string logo { get; set; }
    }



}
