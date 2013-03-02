using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using oikonomos.repositories.interfaces;
using oikonomos.services.interfaces;

namespace oikonomos.services
{
    public class EmailLogger : IEmailLogger
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IPersonRepository _personRepository;

        public EmailLogger(IMessageRepository messageRepository, IPersonRepository personRepository)
        {
            _messageRepository = messageRepository;
            _personRepository = personRepository;
        }

        public void LogSuccess(MailMessage mailMessage, int fromPersonId)
        {
            var toPeopleIds = new List<int>();
            AddPersonIds(mailMessage.To, toPeopleIds);
            AddPersonIds(mailMessage.CC, toPeopleIds);

            _messageRepository.SaveMessage(fromPersonId, toPeopleIds, mailMessage.Subject, mailMessage.Body, "Email", "Success");

        }

        private void AddPersonIds(IEnumerable<MailAddress> addressCollection, List<int> toPeopleIds)
        {
            foreach (var toPersonId in addressCollection.Select(toPerson => _personRepository.FetchPersonIdFromEmailAddress(toPerson.Address)).Where(toPersonId => !toPeopleIds.Contains(toPersonId)))
            {
                toPeopleIds.Add(toPersonId);
            }
        }
    }
}