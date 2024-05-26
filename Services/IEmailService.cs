using Yetai_Eats.Model;

namespace Yetai_Eats.Services
{
    public interface IEmailService
    {
        public Task SendEmailAsync(MailRequest mailRequest);
    }
}
