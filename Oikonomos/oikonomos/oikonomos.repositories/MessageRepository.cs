using System;
using System.Collections.Generic;
using System.Linq;
using oikonomos.data;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
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

        public void SaveMessageRecepient(int messageId, IEnumerable<int> toPersonIds, string messageStatus, string errorMessage)
        {
            foreach (var messageRecepient in toPersonIds.Select(personId => new MessageRecepient
                {
                    MessageSent = DateTime.Now,
                    MessageTo = personId,
                    MessageId = messageId,
                    Status = messageStatus,
                    StatusMessage = errorMessage
                }))
            {
                Context.AddToMessageRecepients(messageRecepient);
            }
            Context.SaveChanges();
        }
    }
}