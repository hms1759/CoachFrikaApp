using CoachFrika.Services;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet("VerifyTransaction")]
    public async Task<IActionResult> VerifyTransaction(string reference)
    {
        // Here you can verify the transaction status
        var verificationResponse = await _paystackService.VerifyTransactionAsync(reference);

        // Process the payment response (e.g., show a success or failure page)
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
}
