using System.Linq;
using oikonomos.common.Models;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class MessageRecepientRepository : RepositoryBase, IMessageRecepientRepository
    {
        public MessageRecepientViewModel FetchMessageRecepient(int messageRecipientId)
        {
            var messageRecepient = Context.MessageRecepients.FirstOrDefault(m => m.MessageRecepientId == messageRecipientId);
            if(messageRecepient==null) return null;
            return new MessageRecepientViewModel
                {
                    MessageRecepientId = messageRecepient.MessageId,
                    MessageToId = messageRecepient.MessageTo,
                    MessageToEmail = messageRecepient.Person.Email,
                    MessageToFullName = messageRecepient.Person.Firstname + " " + messageRecepient.Person.Family.FamilyName,
                    MessageSent = messageRecepient.MessageSent,
                    Status = messageRecepient.Status,
                    StatusMessage = messageRecepient.StatusMessage,
                    Subject = messageRecepient.Message.Subject,
                    Body = messageRecepient.Message.Body

                };
        }
    }
}