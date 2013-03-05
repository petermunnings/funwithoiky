using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace oikonomos.services.interfaces
{
    public interface IEmailLogger
    {
        void LogSuccess(MailMessage mailMessage, string body, int fromPersonId, int churchId);
        void LogError(MailMessage mailMessage, string body, int fromPersonId, Exception exception, int churchId);
    }
}