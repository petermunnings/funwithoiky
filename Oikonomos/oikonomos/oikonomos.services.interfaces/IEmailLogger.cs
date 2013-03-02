using System.Net.Mail;

namespace oikonomos.services.interfaces
{
    public interface IEmailLogger
    {
        void LogSuccess(MailMessage mailMessage, int fromPersonId);
    }
}