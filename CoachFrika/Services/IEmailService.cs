using CoachFrika.Models;

namespace CoachFrika.Services
{
    public interface IEmailService
    {
        //void SendEmail(Message message);
        Task<string> ReadTemplate(string messageType);
        Task<string> SendEmail(Message message);
    }
}
