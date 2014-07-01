using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using oikonomos.repositories;
using oikonomos.repositories.interfaces;
using oikonomos.repositories.interfaces.Messages;
using oikonomos.repositories.Messages;
using oikonomos.services;
using OpenPop.Mime;
using OpenPop.Pop3;

namespace MailChecker
{
    class Program
    {
        private const string Hostname = "sark.aserv.co.za";
        private const int Port = 995;
        private const bool UseSsl = true;
        private const string Username = "reply@oikonomos.co.za";
        private const string Password = "MQaz3xXwzc";
        private static Timer _aTimer;

        static void Main(string[] args)
        {
            _aTimer = new Timer(20000);
            _aTimer.Elapsed += CheckForNewEmails;
            _aTimer.Enabled = true;

            Console.WriteLine("Press the Enter key to exit the program... ");
            Console.ReadLine();
            Console.WriteLine("Terminating the application...");

        }

        private static void CheckForNewEmails(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("{0}: Checking for new messages...", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            using (var pop3Client = new Pop3Client())
            {
                var messages = GetNewMessages(pop3Client);
                var messageCount = messages.Count();
                Console.WriteLine("{0} Messages found", messageCount);
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
                                        Console.WriteLine("Forwarding email on to {0}", originalSender.Email);
                                        emailSender.SendEmail(mm.Subject, mm.Body, Username, originalSender.Email, Username, Password, fromPersonId, originalSender.ChurchId, mm.Attachments);
                                    }
                                    pop3Client.DeleteMessage(count + 1);
                                }
                            }
                            catch (Exception errSending)
                            {
                                Console.WriteLine(errSending.Message);
                            }
                        }

                    }
                }
            }
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
