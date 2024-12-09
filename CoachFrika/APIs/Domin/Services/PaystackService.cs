using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CoachFrika.Services;
using CoachFrika.Common;
using CoachFrika.APIs.ViewModel;

public class PaystackService : IPaystackService
{
    private readonly HttpClient _httpClient;
    private readonly string _paystackSecretKey;

    // Constructor to initialize HttpClient and set the Paystack Secret Key
    public PaystackService(HttpClient httpClient, string paystackSecretKey)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _paystackSecretKey = paystackSecretKey ?? throw new ArgumentNullException(nameof(paystackSecretKey));
    }

    // Method to initialize a transaction
    public async Task<BaseResponse<InitiateModel>> InitializeTransactionAsync(decimal amount, string email)
    {
        var res = new BaseResponse<InitiateModel>();
        res.Status = true;

        var requestData = new
        {
            amount = (int)(amount * 100),  // Convert to Kobo
            email,
            reference = "OLAmi564DE1"  // You can generate or pass a unique reference here
        };

        var jsonContent = JsonConvert.SerializeObject(requestData);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://api.paystack.co/transaction/initialize")
        {
            Content = content
        };

        requestMessage.Headers.Add("Authorization", $"Bearer {_paystackSecretKey}");

        var response = await _httpClient.SendAsync(requestMessage);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject<BaseResponse<InitiateModel>>(responseContent);
            return jsonResponse;
        }
        else
        {
            res.Status = false;
            res.Message = response.ReasonPhrase;
            return res;
        }
    }

    // Method to verify the transaction
    public async Task<BaseResponse<PaymentVerifyData>> VerifyTransactionAsync(string reference)
    {
        var res = new BaseResponse<PaymentVerifyData>();
        res.Status = true;

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://api.paystack.co/transaction/verify/{reference}")
        {
            Headers = { { "Authorization", $"Bearer {_paystackSecretKey}" } }
        };

        var response = await _httpClient.SendAsync(requestMessage);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject<BaseResponse<PaymentVerifyData>>(responseContent);
            return jsonResponse;
        }
        else
        {
            res.Status = false;
            res.Message = response.ReasonPhrase;
            return res;
        }
    }

    // Method to retrieve a transaction by ID
    public async Task<BaseResponse<TransactionData>> RetrieveTransactionAsync(long transactionId)
    {
        var res = new BaseResponse<TransactionData>();
        res.Status = true;

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://api.paystack.co/transaction/{transactionId}")
        {
            Headers = { { "Authorization", $"Bearer {_paystackSecretKey}" } }
        };

        var response = await _httpClient.SendAsync(requestMessage);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject<BaseResponse<TransactionData>>(responseContent);
            return jsonResponse;
        }
        else
        {
            res.Status = false;
            res.Message = response.ReasonPhrase;
            return res;
        }
    }

    // Method to list all transactions
    public async Task<BaseResponse<TransactionList>> ListTransactionsAsync(int page = 1, int perPage = 10)
    {
        var res = new BaseResponse<TransactionList>();
        res.Status = true;

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://api.paystack.co/transaction")
        {
            Headers = { { "Authorization", $"Bearer {_paystackSecretKey}" } }
        };

        var response = await _httpClient.SendAsync(requestMessage);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject<BaseResponse<TransactionList>>(responseContent);
            return jsonResponse;
        }
        else
        {
            res.Status = false;
            res.Message = response.ReasonPhrase;
            return res;
        }
    }

    // Method to initiate a refund
    public async Task<BaseResponse<RefundData>> RefundTransactionAsync(long transactionId, decimal amount)
    {
        var res = new BaseResponse<RefundData>();
        res.Status = true;

        var requestData = new
        {
            transaction = transactionId,
            amount = (int)(amount * 100) // Convert to Kobo
        };

        var jsonContent = JsonConvert.SerializeObject(requestData);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://api.paystack.co/refund")
        {
            Content = content
        };

        requestMessage.Headers.Add("Authorization", $"Bearer {_paystackSecretKey}");

        var response = await _httpClient.SendAsync(requestMessage);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject<BaseResponse<RefundData>>(responseContent);
            return jsonResponse;
        }
        else
        {
            res.Status = false;
            res.Message = response.ReasonPhrase;
            return res;
        }
    }


}
