using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CoachFrika.Services;
using CoachFrika.Common;
using CoachFrika.APIs.ViewModel;
using coachfrikaaaa.Common;
using CoachFrika.Common.Enum;
using Google.Apis.Sheets.v4.Data;
using coachfrikaaaa.APIs.Entity;
using System.ComponentModel;
using CoachFrika.Models;
using CoachFrika.Extensions;

public class PaystackService : IPaystackService
{
    private readonly HttpClient _httpClient;
    private readonly string _paystackSecretKey;
    private readonly AppDbContext _context;
    public readonly IEmailService _emailService;

    // Constructor to initialize HttpClient and set the Paystack Secret Key
    public PaystackService(HttpClient httpClient, string paystackSecretKey, AppDbContext context, IEmailService emailService)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _paystackSecretKey = paystackSecretKey ?? throw new ArgumentNullException(nameof(paystackSecretKey));
        _context = context;
        _emailService = emailService;
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
            reference = $"{Guid.NewGuid()}-{email.Substring(0,5)}"
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
    public async Task<BaseResponse<PaymentVerifyData>> VerifyTransactionAsync(string reference,string logo)
    {

        SentrySdk.CaptureMessage($"VerifyTransactionAsync", level: SentryLevel.Info);
        var res = new BaseResponse<PaymentVerifyData>();
        res.Status = true;

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://api.paystack.co/transaction/verify/{reference}")
        {
            Headers = { { "Authorization", $"Bearer {_paystackSecretKey}" } }
        };

        var response = await _httpClient.SendAsync(requestMessage);

           var payment = _context.Payment.FirstOrDefault(x => x.Paymentrefernce == reference);


        SentrySdk.CaptureMessage($"VerifyTransactionAsync refcode: {payment.Id}", level: SentryLevel.Info);
        if (response.IsSuccessStatusCode)
        {

            SentrySdk.CaptureMessage($"VerifyTransactionAsync IsSuccessStatusCode", level: SentryLevel.Info);
            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject<BaseResponse<PaymentVerifyData>>(responseContent);
            if (payment != null)
            {
                payment.PaymentStatus = PaymentStatus.Approved;

                var teach = _context.CoachFrikaUsers.FirstOrDefault(x => x.Email == payment.CreatedBy);
                if(teach != null)
                {
                    teach.Stages = 6;
                    teach.hasPaid = true;
                }
              await  SendPaymentNotificationEmail(teach, logo);
                _context.SaveChanges();
            }

            return jsonResponse;
        }
        else
        {
            SentrySdk.CaptureMessage($"VerifyTransactionAsync ReasonPhrase: {response.ReasonPhrase}", level: SentryLevel.Info);
            if (payment != null)
            {
                payment.PaymentStatus = PaymentStatus.Cancelled;
            }
            _context.SaveChanges();
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
    public async Task<TransactionList> ListTransactionsAsync(int page = 1, int perPage = 10)
    {
        var res = new TransactionList();
        try {
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://api.paystack.co/transaction")
        {
            Headers = { { "Authorization", $"Bearer {_paystackSecretKey}" } }
        };

        var response = await _httpClient.SendAsync(requestMessage);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject<TransactionList>(responseContent);

            return jsonResponse;
        }
        else
        {
            res.status = false;
            res.message = response.ReasonPhrase;
            return res;
            }
        }
        catch (Exception ex)
        {
            res.status = false;
            res.message = ex.Message;
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

    public async Task<string> WebHooksVerification(PaymentWebHook model)
    {  if (model == null)
        throw new NotImplementedException();
        SentrySdk.CaptureMessage($"WebHooksVerification service {JsonConvert.SerializeObject(model)}", level: SentryLevel.Info);
      
        if(model.data == null)
            throw new NotImplementedException();
        var response = model.data;
        if(response.status != "success")
            throw new NotImplementedException();

        var payment = _context.Payment.FirstOrDefault(x => x.Paymentrefernce == response.reference);

        if (payment == null)
            throw new NotImplementedException();

        payment.PaymentStatus = PaymentStatus.Approved;

        var teach = _context.CoachFrikaUsers.FirstOrDefault(x => x.Email == payment.CreatedBy);
        if (teach != null)
        {
            teach.Stages = 6;
        }
        await SendPaymentNotificationEmail(teach, model.logo);
        _context.SaveChanges();
        return response.status;
    }


    private async Task SendPaymentNotificationEmail(CoachFrikaUsers user, string logoUrl)
    {
        var subject = "Payment Notification";
        var body = $"Your Payment has been recieved, Kindly logout and login again";

        var bodyTemplate = await _emailService.ReadTemplate("forgetPassword");
        //inserting variable
        var messageToParse = new Dictionary<string, string>
                    {
                        { "{Fullname}", user.FullName},
                        { "{Message}", body},
                        { "{logo}", logoUrl},
                    };

        //  email notification
        var messageBody = bodyTemplate.ParseTemplate(messageToParse);
        var message = new Message(new List<string> { user.Email }, subject, messageBody);

        await _emailService.SendEmail(message);
    }

}
