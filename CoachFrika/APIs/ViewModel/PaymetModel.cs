using Serilog;
using System.Net;

namespace CoachFrika.APIs.ViewModel
{

    // Model classes for JSON response parsing

    public class InitiateModel
    {
        public string authorization_url { get; set; }
        public string access_code { get; set; }
        public string reference { get; set; }
    }

    public class PaymentVerifyData
    {
        public long id { get; set; }
        public string status { get; set; }
        public string reference { get; set; }
        public string message { get; set; }
        public int amount { get; set; }
    }

    public class TransactionData
    {
        public long id { get; set; }
        public string status { get; set; }
        public string reference { get; set; }
        public int amount { get; set; }
        public DateTime created_at { get; set; }
        public string currency { get; set; }
    }
   
    //public class TransactionList
    //{
    //    public List<TransactionsDTO> data { get; set; }
    //    public bool status { get; set; }
    //    public string message { get; set; }
    //}

    public class RefundData
    {
        public string status { get; set; }
        public string message { get; set; }
    }


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Authorization
    {
        public string authorization_code { get; set; }
        public string bin { get; set; }
        public string last4 { get; set; }
        public string exp_month { get; set; }
        public string exp_year { get; set; }
        public string channel { get; set; }
        public string card_type { get; set; }
        public string bank { get; set; }
        public string country_code { get; set; }
        public string brand { get; set; }
        public bool reusable { get; set; }
        public string signature { get; set; }
        public object account_name { get; set; }
    }

    public class Customer
    {
        public int id { get; set; }
        public object first_name { get; set; }
        public object last_name { get; set; }
        public string email { get; set; }
        public object phone { get; set; }
        public object metadata { get; set; }
        public string customer_code { get; set; }
        public string risk_action { get; set; }
    }

    public class Datum
    {
        public object id { get; set; }
        public string domain { get; set; }
        public string status { get; set; }
        public string reference { get; set; }
        public int amount { get; set; }
        public object message { get; set; }
        public string gateway_response { get; set; }
        public DateTime? paid_at { get; set; }
        public DateTime created_at { get; set; }
        public string channel { get; set; }
        public string currency { get; set; }
        public string ip_address { get; set; }
        public object metadata { get; set; }
        public Log log { get; set; }
        public int? fees { get; set; }
        public object fees_split { get; set; }
        public Customer customer { get; set; }
        public Authorization authorization { get; set; }
        public Plan plan { get; set; }
        public Split split { get; set; }
        public Subaccount subaccount { get; set; }
        public object order_id { get; set; }
        public DateTime? paidAt { get; set; }
        public DateTime createdAt { get; set; }
        public int requested_amount { get; set; }
        public Source source { get; set; }
        public object connect { get; set; }
        public object pos_transaction_data { get; set; }
    }

    public class History
    {
        public string type { get; set; }
        public string message { get; set; }
        public int time { get; set; }
    }

    public class Log
    {
        public int start_time { get; set; }
        public int time_spent { get; set; }
        public int attempts { get; set; }
        public int errors { get; set; }
        public bool success { get; set; }
        public bool mobile { get; set; }
        public List<object> input { get; set; }
        public List<History> history { get; set; }
        public string authentication { get; set; }
    }

    public class Meta
    {
        public int total { get; set; }
        public int total_volume { get; set; }
        public int skipped { get; set; }
        public int perPage { get; set; }
        public int page { get; set; }
        public int pageCount { get; set; }
    }

    public class Plan
    {
    }

    public class TransactionList
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<Datum> data { get; set; }
        public Meta meta { get; set; }
    }

    public class Source
    {
        public string source { get; set; }
        public string type { get; set; }
        public object identifier { get; set; }
        public string entry_point { get; set; }
    }

    public class Split
    {
    }

    public class Subaccount
    {
    }


}
