using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http;
using oikonomos.repositories;
using oikonomos.repositories.interfaces;
using oikonomos.repositories.interfaces.Messages;
using oikonomos.repositories.Messages;
using oikonomos.services;
using OpenPop.Mime;
using OpenPop.Pop3;

namespace oikonomos.web.ApiControllers
{
    public class CheckForEmailController : ApiController
    {
        private const string Hostname = "sark.aserv.co.za";
        private const int Port = 995;
        private const bool UseSsl = true;
        private const string Username = "reply@oikonomos.co.za";
        private const string Password = "MQaz3xXwzc";
        
        public IEnumerable<string> Get()
        {
            var returnMessages = new List<string>();
            using (var pop3Client = new Pop3Client())
            {
                var messages = GetNewMessages(pop3Client);
                var messageCount = messages.Count();
                if (messageCount > 0)
                {
                    IMessageRepository messageRepository = new MessageRepository();
                    IMessageRecepientRepository messageRecepientRepository = new MessageRecepientRepository();
                    IMessageAttachmentRepository messageAttachmentRepository = new MessageAttachmentRepository();
                    IPermissionRepository permissionRepository = new PermissionRepository();
                    IChurchRepository churchRepository = new ChurchRepository();
                    IPersonRepository personRepository = new PersonRepository(permissionRepository, churchRepository);

                    var emailSender = new EmailSender(messageRepository, messageRecepientRepository, messageAttachmentRepository, personRepository);

                    for (var count = 0; count < messageCount; count++)
                    {
                        var mm = messages[count].ToMailMessage();
                        var regex = new Regex(@"##([0-9]*)##");
                        var matches = regex.Matches(mm.Body);
                        if (matches.Count > 0 && matches[0].Groups.Count > 1)
                        {
                            try
                            {
                                int messageId;
                                if (int.TryParse(matches[0].Groups[1].Value, out messageId))
                                {
                                    var originalSender = messageRepository.GetSender(messageId);
                                    if (originalSender != null)
                                    {
                                        var originalReceiver = personRepository.FetchPersonIdsFromEmailAddress(mm.From.Address, originalSender.ChurchId);
                                        var fromPersonId = originalSender.PersonId;

                                        if (originalReceiver.Any())
                                        {
                                            fromPersonId = originalReceiver.First();
                                        }
                                        returnMessages.Add(string.Format("Forwarding email on to {0}", originalSender.Email));
                                        emailSender.SendEmail(mm.Subject, mm.Body, Username, originalSender.Email, Username, Password, fromPersonId, originalSender.ChurchId, mm.Attachments);
                                    }
                                    pop3Client.DeleteMessage(count + 1);
                                }
                            }
                            catch (Exception errSending)
                            {
                                returnMessages.Add(errSending.Message);
                                emailSender.SendExceptionEmailAsync(errSending);
                            }
                        }
                    }
                }
            }
            return returnMessages;
        }

        private static List<Message> GetNewMessages(Pop3Client pop3Client)
        {
            var allMessages = new List<Message>();

            pop3Client.Connect(Hostname, Port, UseSsl);
            pop3Client.Authenticate(Username, Password);
            var messageCount = pop3Client.GetMessageCount();

            for (var i = messageCount; i > 0; i--)
            {
                allMessages.Add(pop3Client.GetMessage(i));
            }
            return allMessages;
        }
    }
}