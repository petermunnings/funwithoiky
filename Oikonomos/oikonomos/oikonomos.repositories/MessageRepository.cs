using System;
using System.Collections.Generic;
using System.Linq;
using oikonomos.data;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class MessageRepository : RepositoryBase, IMessageRepository
    {
        public void SaveMessage(int fromPersonId, IEnumerable<int> toPeopleIds, string subject, string body, string messageType, string messageStatus)
        {
            var message = new Message
                {
                    Body = body,
                    Subject = subject,
                    MessageFrom = fromPersonId,
                    MessageType = messageType
                };
            Context.AddToMessages(message);

            foreach (var messageRecepient in toPeopleIds.Select(personId => new MessageRecepient
                {
                    MessageSent = DateTime.Now,
                    MessageTo = personId,
                    Message = message,
                    Status = messageStatus
                }))
            {
                Context.AddToMessageRecepients(messageRecepient);
            }
            Context.SaveChanges();
        }
    }
}