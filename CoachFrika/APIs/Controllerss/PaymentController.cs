using Microsoft.AspNetCore.Mvc;

public class PaymentController : Controller
{
    private readonly PaystackService _paystackService;

    public PaymentController(PaystackService paystackService)
    {
        _paystackService = paystackService;
    }

    [HttpPost("pay")]
    public async Task<IActionResult> Pay(decimal amount, string email)
    {
        var transactionUrl = await _paystackService.InitializeTransactionAsync(amount, email);

        // Redirect the user to Paystack for payment
        return Redirect(transactionUrl);
    }

    [HttpGet("payment/callback")]
    public async Task<IActionResult> PaymentCallback(string reference)
    {
        // Here you can verify the transaction status
        var verificationResponse = await _paystackService.VerifyTransactionAsync(reference);

        // Process the payment response (e.g., show a success or failure page)
        return View("PaymentResult", verificationResponse);
    }
}
