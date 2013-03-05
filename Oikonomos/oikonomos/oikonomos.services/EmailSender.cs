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

            using (var message = new MailMessage())
            {
                try
                {
                    message.From = new MailAddress(login);
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;
                    foreach (var emailTo in emailAddressTo)
                    {
                        message.To.Add(emailTo); 
                    }
                    SendEmail(message, login, password, displayFrom);
                    _emailLogger.LogSuccess(message, body, personIdFrom, churchId);
                }
                catch (Exception e)
                {
                    _emailLogger.LogError(message, body, 1, e, churchId);
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
                    _emailLogger.LogError(message, "Exception from website", 1, e, 1);
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
                    _emailLogger.LogError(message, HttpUtility.HtmlDecode(body), 1, e, 1);
                }
            }
        }

        private void SendEmail(MailMessage message, string username, string password, string displayName)
        {
            message.From = new MailAddress(username, displayName);

            using (var client = new SmtpClient("mail.oikonomos.co.za"))
            {
                client.Credentials = new System.Net.NetworkCredential(username, password);
                AddDisclaimer(message, displayName);
                client.Send(message);
            }
        }

        private static void AddDisclaimer(MailMessage message, string displayName)
        {
            message.Body = "<body style='font-family:Verdana'>" + message.Body;
            var disclaimer = "<p>&nbsp;</p><hr />";
            disclaimer += string.Format("<p style='font-size:9px'>Please do not reply to this email.  This email was sent by {0}</p>", displayName);
            message.Body += disclaimer;
            message.Body += "</body>";
        }
    }
}