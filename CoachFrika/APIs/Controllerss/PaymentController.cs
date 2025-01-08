using CoachFrika.APIs.ViewModel;
using CoachFrika.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


public class PaymentController : Controller
{
    private readonly IPaystackService _paystackService;

    // Constructor
    public PaymentController(IPaystackService paystackService)
    {
        _paystackService = paystackService;
    }

    [HttpPost("pay")]
    public async Task<IActionResult> Pay(decimal amount, string email)
    {
        var transactionUrl = await _paystackService.InitializeTransactionAsync(amount, email);

        // Redirect the user to Paystack for payment
        return Ok(transactionUrl);
    }

    [HttpPut("VerifyTransaction")]
    public async Task<IActionResult> VerifyTransaction(string reference)
    {
        var logoUrl = $"{Request.Scheme}://{Request.Host}/images/logo.png";
        var verificationResponse = await _paystackService.VerifyTransactionAsync(reference, logoUrl);

        return Ok(verificationResponse);
    }


    [HttpGet("RetrieveTransactionAsync")]
    public async Task<IActionResult> RetrieveTransactionAsync(long transactionId)
    {
        // Here you can verify the transaction status
        var verificationResponse = await _paystackService.RetrieveTransactionAsync(transactionId);

        // Process the payment response (e.g., show a success or failure page)
        return Ok(verificationResponse);
    }

    [HttpGet("RefundTransactionAsync")]
    public async Task<IActionResult> RefundTransactionAsync(long transactionId, decimal Amount)
    {
        // Here you can verify the transaction status
        var verificationResponse = await _paystackService.RefundTransactionAsync(transactionId, Amount);

        // Process the payment response (e.g., show a success or failure page)
        return Ok(verificationResponse);
    }
    [HttpGet("ListTransactionsAsync")]
    public async Task<IActionResult> ListTransactionsAsync(int page, int pageSize)
    {
        // Here you can verify the transaction status
        var verificationResponse = await _paystackService.ListTransactionsAsync(page, pageSize);

        // Process the payment response (e.g., show a success or failure page)
        return Ok(verificationResponse);
    }

    [AllowAnonymous]
    [HttpPost("ProcessPaystack")]
    public async Task<IActionResult> WebHooksVerification([FromBody]PaymentWebHook model)
    {
        // Serialize the model to JSON and capture it in Sentry
        string modelJson = JsonConvert.SerializeObject(model);
        SentrySdk.CaptureMessage($"WebHooksVerification received with model: {modelJson}", level: SentryLevel.Info);

        //SentrySdk.CaptureMessage($"WebHooksVerification", level: SentryLevel.Info);
        string logoUrl = $"{Request.Scheme}://{Request.Host}/images/logo.png";
        model.logo = logoUrl;
        var verificationResponse =await _paystackService.WebHooksVerification(model);
        return Ok(verificationResponse);
    }

}
