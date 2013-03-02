using System;
using System.Net.Mail;

namespace oikonomos.services.interfaces
{
    public interface IEmailLogger
    {
        void LogSuccess(MailMessage mailMessage, int fromPersonId, int churchId);
        void LogError(MailMessage mailMessage, int fromPersonId, Exception exception, int churchId);
    }
}