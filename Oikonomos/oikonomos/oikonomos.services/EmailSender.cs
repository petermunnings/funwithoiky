using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public string QueueEmails(string subject, string body, string displayFrom, IEnumerable<string> emailAddressTo, string login, string password, int personIdFrom, int churchId, IEnumerable<UploadFilesResult> attachmentList)
        {
            var returnMessage = string.Empty;
            try
            {
                var messageId = _messageRepository.SaveMessage(personIdFrom, subject, body, "Email");
                if (attachmentList != null)
                {
                    foreach (var attachment in attachmentList)
                    {
                        _messageAttachmentRepository.SaveMessageAttachment(messageId, attachment.Name, attachment.Type,
                            attachment.Length, attachment.AttachmentContentType, attachment.AttachmentContent);
                    }
                }
                foreach (var emailTo in emailAddressTo)
                {
                    try
                    {
                        _messageRecepientRepository.SaveMessageRecepient(messageId,
                            _personRepository.FetchPersonIdsFromEmailAddress(emailTo, churchId), MessageStatus.Queued,
                            string.Empty);
                    }
                    catch (Exception e)
                    {
                        _messageRecepientRepository.SaveMessageRecepient(messageId,
                            _personRepository.FetchPersonIdsFromEmailAddress(emailTo, churchId), MessageStatus.Failed,
                            e.Message);
                        returnMessage += "There was a problem queueing the message to " + emailTo + ": e.Message";
                    }
                }
            }
            catch (Exception exError)
            {
                SendExceptionEmailAsync(exError);
                return exError.Message;
            }
            if (returnMessage == string.Empty)
                return "Message succesfully queued to " + emailAddressTo.Count() + " email addresses";
            return returnMessage;
        }

        public string SendEmail(string subject, string body, string displayName, string emailTo, string login, string password, int personIdFrom, int churchId, IEnumerable<Attachment> attachmentCollection, int? messageId = null, int? messageRecepientId = null)
        {
            var returnMessage = string.Empty;
            try
            {
                
                if(messageId==null)
                   messageId = _messageRepository.SaveMessage(personIdFrom, subject, body, "Email");

                try
                {
                    using (var message = new MailMessage())
                    {
                        message.From = new MailAddress(login, displayName);
                        message.Subject = subject;
                        message.Body = body;
                        message.IsBodyHtml = true;
                        if (attachmentCollection != null)
                        {
                            foreach (var attachment in attachmentCollection)
                            {
                                message.Attachments.Add(attachment);
                            }
                        }

                        message.To.Add(emailTo);
                        SendEmail(message, login, password, messageId.Value);
                        if (messageRecepientId == null)
                            _messageRecepientRepository.SaveMessageRecepient(messageId.Value,
                                _personRepository.FetchPersonIdsFromEmailAddress(emailTo, churchId),
                                MessageStatus.Success, string.Empty);
                        else
                            _messageRecepientRepository.UpdateMessageRecepient(messageRecepientId.Value,
                                MessageStatus.Success);
                    }
                }
                catch (Exception e)
                {
                    if (messageRecepientId == null)
                        _messageRecepientRepository.SaveMessageRecepient(messageId.Value,
                            _personRepository.FetchPersonIdsFromEmailAddress(emailTo, churchId), MessageStatus.Failed,
                            e.Message);
                    else
                        _messageRecepientRepository.UpdateMessageRecepient(messageRecepientId.Value,
                            MessageStatus.Failed, e.Message);
                    returnMessage += e.Message;
                }
            }
            catch (Exception exError)
            {
                
                SendExceptionEmailAsync(exError);
                return exError.Message;
            }
            return returnMessage == string.Empty ? "Message succesfully sent to " + emailTo : returnMessage;
        }

        public void SendExceptionEmailAsync(Exception ex)
        {
            using (var message = new MailMessage())
            {
                try
                {
                    message.Subject = "Exception from website";
                    message.Body = ex.ToString();
                    message.From = new MailAddress("support@oikonomos.co.za", "Oiky Support");

                    message.To.Add("peter@munnings.co.za");
                    message.IsBodyHtml = false;

                    SendEmail(message, "support@oikonomos.co.za", "sandton2000", 0);
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
                    message.From = new MailAddress("support@oikonomos.co.za", "Oiky support");
                    message.Subject = subject;
                    message.Body = HttpUtility.HtmlDecode(body);

                    message.To.Add("peter@munnings.co.za");
                    message.IsBodyHtml = true;

                    SendEmail(message, "support@oikonomos.co.za", "sandton2000", 0);

                }
                catch (Exception)
                {
                    //There is a problem creating the email...
                }
            }
        }

        private static void SendEmail(MailMessage message, string username, string password, int messageId)
        {
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
                    message.Body = message.Body.Replace("</body>", string.Format("<p style='font-size:7px; color:white'>##{0}##</p>", messageId) + "</body>");
                }
                else
                {
                    message.Body = "<body style='font-family:Verdana'>" + message.Body;
                    var messageIdSection = "<p>&nbsp;</p>";
                    messageIdSection += string.Format("<p style='font-size:7px; color:white'>##{0}##</p>", messageId);
                    message.Body += messageIdSection;
                    message.Body += "</body>";
                }
            }
        }
    }
}