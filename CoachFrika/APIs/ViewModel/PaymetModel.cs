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

    public class TransactionList
    {
        public TransactionData[] data { get; set; }
        public bool status { get; set; }
    }

    public class RefundData
    {
        public string status { get; set; }
        public string message { get; set; }
    }
}
