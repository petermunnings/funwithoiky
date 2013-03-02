using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Web;
using oikonomos.services.interfaces;

namespace oikonomos.services
{
    public class EmailSender : IEmailSender
    {
        private readonly IEmailLogger _emailLogger;

        public EmailSender(IEmailLogger emailLogger)
        {
            _emailLogger = emailLogger;
        }

        public void SendEmail(string subject, string body, string displayFrom, IEnumerable<string> emailAddressTo, string login, string password, int personIdFrom, int churchId)
        {
            foreach (var emailTo in emailAddressTo)
            {
                using (var message = new MailMessage(login, emailTo, subject, body))
                {
                    try
                    {
                        message.IsBodyHtml = true;
                        SendEmail(message, login, password, displayFrom);
                        _emailLogger.LogSuccess(message, personIdFrom, churchId);
                    }
                    catch (Exception e)
                    {
                        _emailLogger.LogError(message, 1, e, churchId);
                    }
                }
            }
        }

        public void SendExceptionEmailAsync(Exception ex)
        {
            using (var message = new MailMessage())
            {
                try
                {
                    message.Subject = "Exception from website";
                    message.Body = ex.ToString();

                    message.To.Add("peter@munnings.co.za");
                    message.IsBodyHtml = false;

                    SendEmail(message, "support@oikonomos.co.za", "sandton2000", "Oiky Error");
                }
                catch (Exception e)
                {
                    _emailLogger.LogError(message, 1, e, 1);
                }
            }
        }

        public void SendSystemEmailAsync(string subject, string body)
        {
            using (var message = new MailMessage())
            {
                try
                {
                    message.Subject = subject;
                    message.Body = HttpUtility.HtmlDecode(body);

                    message.To.Add("peter@munnings.co.za");
                    message.IsBodyHtml = true;

                    SendEmail(message, "support@oikonomos.co.za", "sandton2000", "Oiky System Message");

                }
                catch (Exception e)
                {
                    _emailLogger.LogError(message, 1, e, 1);
                }
            }
        }

        private void SendEmail(MailMessage message, string username, string password, string displayName)
        {
            message.From = new MailAddress(username, displayName);

            using (var client = new SmtpClient("mail.oikonomos.co.za"))
            {
                client.Credentials = new System.Net.NetworkCredential(username, password);
                client.Send(message);
            }
        }
    }
}