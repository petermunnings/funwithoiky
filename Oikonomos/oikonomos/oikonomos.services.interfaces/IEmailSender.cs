using System;
using System.Collections.Generic;

namespace oikonomos.services.interfaces
{
    public interface IEmailSender
    {
        void SendEmail(string subject, string body, string displayName, IEnumerable<string> emailTo, string login, string password, int personIdFrom, int churchId);
        void SendExceptionEmailAsync(Exception ex);
        void SendSystemEmailAsync(string subject, string body);
    }
}