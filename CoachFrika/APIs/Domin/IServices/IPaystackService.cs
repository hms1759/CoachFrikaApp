namespace CoachFrika.Services
{
    public interface IPaystackService
    {
        Task<string> InitializeTransactionAsync(decimal amount, string email);
        Task<string> VerifyTransactionAsync(string reference);
    }
}
