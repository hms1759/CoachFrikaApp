using System.Text.Json.Serialization;

namespace CoachFrika.APIs.ViewModel
{

    public class Data
    {
        public int id { get; set; }
        public string domain { get; set; }
        public string status { get; set; }
        public string reference { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public DateTime paid_at { get; set; }
        public DateTime transaction_date { get; set; }
        public string channel { get; set; }
        public Metadata metadata { get; set; }
        public string message { get; set; }
        public string gateway_response { get; set; }
        public bool paid { get; set; }
        public Authorization authorization { get; set; }
        public Customer customer { get; set; }
        public string order_id { get; set; }
    }

    public class Metadata
    {
        public string order_number { get; set; }
    }

    public class PaymentWebHook
    {
        public string @event { get; set; }
        public Data data { get; set; }
        [JsonIgnore]
        public string  logo { get; set; }
    }

}
