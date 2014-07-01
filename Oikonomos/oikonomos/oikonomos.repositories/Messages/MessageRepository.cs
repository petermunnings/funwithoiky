using System.Linq;
using oikonomos.common.DTOs;
using oikonomos.common.Models;
using oikonomos.data;
using oikonomos.repositories.interfaces.Messages;

namespace oikonomos.repositories.Messages
{
    public class MessageRepository : RepositoryBase, IMessageRepository
    {
        public int SaveMessage(int fromPersonId, string subject, string body, string messageType)
        {
            var message = new Message
            {
                Body = body,
                Subject = subject,
                MessageFrom = fromPersonId,
                MessageType = messageType
            };
            Context.AddToMessages(message);
            Context.SaveChanges();
            return message.MessageId;
        }

        public PersonDto GetSender(int messageId)
        {
            var message = Context.Messages.FirstOrDefault(m => m.MessageId == messageId);
            if (message == null) return null;
            var person = Context.People.FirstOrDefault(p => p.PersonId == message.MessageFrom);
            if (person == null) return null;
            return new PersonDto
            {
                PersonId = person.PersonId,
                Email = person.Email,
                ChurchId = person.PersonChurches.First().ChurchId
            };
        }
    }
}