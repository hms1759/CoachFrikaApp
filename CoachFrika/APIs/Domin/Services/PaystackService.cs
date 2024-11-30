//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;
//using CoachFrika.Services;
//using Newtonsoft.Json;

//public class PaystackService : IPaystackService
//{
//    private readonly HttpClient _httpClient;
//    private readonly string _paystackSecretKey;

//    // Constructor to initialize HttpClient and set the Paystack Secret Key
//    public PaystackService(HttpClient httpClient, string paystackSecretKey)
//    {
//        _httpClient = httpClient;
//        _paystackSecretKey = paystackSecretKey;
//    }

//    // Method to initialize a transaction
//    public async Task<string> InitializeTransactionAsync(decimal amount, string email)
//    {

//        var requestData = new
//        {
//            amount = (int)(amount * 100),  // Convert to Kobo
//            email = email,
//            // Optionally, you can pass other params such as the callback URL
//            callback_url = "https://yourapp.com/payment/callback"  // Paystack will redirect to this URL after payment
//        };

//        var jsonContent = JsonConvert.SerializeObject(requestData);
//        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

//        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://api.paystack.co/transaction/initialize")
//        {
//            Content = content
//        };

//        requestMessage.Headers.Add("Authorization", $"Bearer {_paystackSecretKey}");

//        var response = await _httpClient.SendAsync(requestMessage);

//        if (response.IsSuccessStatusCode)
//        {
//            var responseContent = await response.Content.ReadAsStringAsync();
//            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);

//            // You will get a URL to redirect the user for payment
//            return jsonResponse.data.authorization_url;
//        }
//        else
//        {
//            return $"Error: {response.ReasonPhrase}";
//        }
//    }

//    // Method to verify the transaction
//    public async Task<string> VerifyTransactionAsync(string reference)
//    {
//        var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://api.paystack.co/transaction/verify/{reference}")
//        {
//            Headers = { { "Authorization", $"Bearer {_paystackSecretKey}" } }
//        };

//        var response = await _httpClient.SendAsync(requestMessage);

//        if (response.IsSuccessStatusCode)
//        {
//            var responseContent = await response.Content.ReadAsStringAsync();
//            return responseContent;  // You can parse this JSON for more details
//        }
//        else
//        {
//            return $"Error: {response.ReasonPhrase}";
//        }
//    }
//}
