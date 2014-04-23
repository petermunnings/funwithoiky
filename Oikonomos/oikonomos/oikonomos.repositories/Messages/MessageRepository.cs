using System;
using System.Collections.Generic;
using System.Linq;
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

        
    }
}