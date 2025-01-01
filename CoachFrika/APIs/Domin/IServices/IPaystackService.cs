using CoachFrika.APIs.ViewModel;
using CoachFrika.Common;
using static PaystackService;

namespace CoachFrika.Services
{
    public interface IPaystackService
    {
        Task<BaseResponse<InitiateModel>> InitializeTransactionAsync(decimal amount, string email);
        Task<BaseResponse<PaymentVerifyData>> VerifyTransactionAsync(string reference);
        Task<BaseResponse<TransactionData>> RetrieveTransactionAsync(long transactionId);
        Task<BaseResponse<RefundData>> RefundTransactionAsync(long transactionId, decimal amount);
        Task<TransactionList> ListTransactionsAsync(int page = 1, int perPage = 10);
    }
}
