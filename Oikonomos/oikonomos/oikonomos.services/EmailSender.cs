using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using oikonomos.common.DTOs;
using oikonomos.repositories.interfaces;
using oikonomos.repositories.interfaces.Messages;
using oikonomos.services.interfaces;

namespace oikonomos.services
{
    public class EmailSender : IEmailSender
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMessageRecepientRepository _messageRecepientRepository;
        private readonly IMessageAttachmentRepository _messageAttachmentRepository;
        private readonly IPersonRepository _personRepository;

        public EmailSender(
            IMessageRepository messageRepository,
            IMessageRecepientRepository messageRecepientRepository,
            IMessageAttachmentRepository messageAttachmentRepository,
            IPersonRepository personRepository)
        {
            _messageRepository = messageRepository;
            _messageRecepientRepository = messageRecepientRepository;
            _messageAttachmentRepository = messageAttachmentRepository;
            _personRepository = personRepository;
        }

        public void SendEmail(string subject, string body, string displayFrom, IEnumerable<string> emailAddressTo, string login, string password, int personIdFrom, int churchId, IEnumerable<UploadFilesResult> attachmentList)
        {
            try
            {
                var messageId = _messageRepository.SaveMessage(personIdFrom, subject, body, "Email");

                foreach (var emailTo in emailAddressTo)
                {
                    try
                    {
                        using (var message = new MailMessage())
                        {
                            message.From = new MailAddress(login);
                            message.Subject = subject;
                            message.Body = body;
                            message.IsBodyHtml = true;
                            if (attachmentList != null)
                            {
                                foreach (var attachment in attachmentList)
                                {
                                    var msgAttachment = new Attachment(new MemoryStream(attachment.AttachmentContent), attachment.Name, attachment.AttachmentContentType);
                                    _messageAttachmentRepository.SaveMessageAttachment(messageId, attachment.Name, attachment.Type, attachment.Length, attachment.AttachmentContentType, attachment.AttachmentContent);
                                    message.Attachments.Add(msgAttachment);
                                }
                            }

                            message.To.Add(emailTo);
                            SendEmail(message, login, password, displayFrom, messageId);
                            _messageRecepientRepository.SaveMessageRecepient(messageId, _personRepository.FetchPersonIdsFromEmailAddress(emailTo, churchId),"Success", string.Empty);
                        }
                    }
                    catch (Exception e)
                    {
                        _messageRecepientRepository.SaveMessageRecepient(messageId, _personRepository.FetchPersonIdsFromEmailAddress(emailTo, churchId), "Failure", e.Message);
                    }
                }
            }
            catch (Exception)
            {
                //There is a problem creating the email...
            }
        }

        public void SendEmail(string subject, string body, string displayName, string emailTo, string login,
            string password, int personIdFrom, int churchId, AttachmentCollection attachmentCollection)
        {
            try
            {
                var messageId = _messageRepository.SaveMessage(personIdFrom, subject, body, "Email");

                try
                {
                    using (var message = new MailMessage())
                    {
                        message.From = new MailAddress(login);
                        message.Subject = subject;
                        message.Body = body;
                        message.IsBodyHtml = true;
                        if (attachmentCollection != null)
                        {
                            foreach (var attachment in attachmentCollection)
                            {
                                message.Attachments.Add(attachment);
                                //Need to save the attachements
                            }
                        }

                        message.To.Add(emailTo);
                        SendEmail(message, login, password, displayName, messageId);
                        _messageRecepientRepository.SaveMessageRecepient(messageId, _personRepository.FetchPersonIdsFromEmailAddress(emailTo, churchId), "Success", string.Empty);
                    }
                }
                catch (Exception e)
                {
                    _messageRecepientRepository.SaveMessageRecepient(messageId, _personRepository.FetchPersonIdsFromEmailAddress(emailTo, churchId), "Failure", e.Message);
                }
            }
            catch (Exception)
            {
                //There is a problem creating the email...
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

                    SendEmail(message, "support@oikonomos.co.za", "sandton2000", "Oiky Error", 0);
                }
                catch (Exception)
                {
                    //There is a problem creating the email...
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

                    SendEmail(message, "support@oikonomos.co.za", "sandton2000", "Oiky System Message", 0);

                }
                catch (Exception)
                {
                    //There is a problem creating the email...
                }
            }
        }

        private static void SendEmail(MailMessage message, string username, string password, string displayName, int messageId)
        {
            message.From = new MailAddress(username, displayName);

            using (var client = new SmtpClient("mail.oikonomos.co.za"))
            {
                client.Credentials = new System.Net.NetworkCredential(username, password);
                AddMessageId(message, messageId);
                client.Send(message);
            }
        }

        private static void AddMessageId(MailMessage message, int messageId)
        {
            const string pattern = @"##([0-9]*)##";
            var regex = new Regex(pattern);
            var matches = regex.Matches(message.Body);
            if (matches.Count > 0 && matches[0].Groups.Count > 1)
            {
                message.Body = Regex.Replace(message.Body, pattern, string.Format("##{0}##", messageId));
            }
            else
            {
                if (message.Body.Contains("</body>"))
                {
                    message.Body = message.Body.Replace("</body>", string.Format("<p style='font-size:7px'>##{0}##</p>", messageId) + "</body>");
                }
                else
                {
                    message.Body = "<body style='font-family:Verdana'>" + message.Body;
                    var messageIdSection = "<p>&nbsp;</p><hr />";
                    messageIdSection += string.Format("<p style='font-size:7px'>##{0}##</p>", messageId);
                    message.Body += messageIdSection;
                    message.Body += "</body>";
                }
            }
        }
    }
}