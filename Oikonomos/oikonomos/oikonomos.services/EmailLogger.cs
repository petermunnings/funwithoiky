using System;
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

        public void LogSuccess(MailMessage mailMessage, int fromPersonId, int churchId)
        {
            var toPeopleIds = new List<int>();
            AddPersonIds(mailMessage.To, toPeopleIds, churchId);
            AddPersonIds(mailMessage.CC, toPeopleIds, churchId);

            _messageRepository.SaveMessage(fromPersonId, toPeopleIds, mailMessage.Subject, mailMessage.Body, "Email", "Success");

        }

        public void LogError(MailMessage mailMessage, int fromPersonId, Exception exception, int churchId)
        {
            var toPeopleIds = new List<int>();
            AddPersonIds(mailMessage.To, toPeopleIds, churchId);
            AddPersonIds(mailMessage.CC, toPeopleIds, churchId);

            _messageRepository.SaveMessage(fromPersonId, toPeopleIds, mailMessage.Subject, mailMessage.Body, "Email", "Failure", exception.Message);

        }

        private void AddPersonIds(IEnumerable<MailAddress> addressCollection, List<int> toPeopleIds, int churchId)
        {
            foreach (var address in addressCollection)
            {
                var personIds = _personRepository.FetchPersonIdsFromEmailAddress(address.Address, churchId);
                foreach (var id in personIds.Where(id => !toPeopleIds.Contains(id)))
                {
                    toPeopleIds.Add(id);
                }
            }
        }
    }
}