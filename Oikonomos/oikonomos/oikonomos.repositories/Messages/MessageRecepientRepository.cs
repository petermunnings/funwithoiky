using System;
using System.Collections.Generic;
using System.Linq;
using oikonomos.common;
using oikonomos.common.Models;
using oikonomos.data;
using oikonomos.repositories.interfaces.Messages;

namespace oikonomos.repositories.Messages
{
    public class MessageRecepientRepository : RepositoryBase, IMessageRecepientRepository
    {
        public MessageRecepientViewModel FetchMessageRecepient(int messageRecipientId)
        {
            var messageRecepient = Context.MessageRecepients.FirstOrDefault(m => m.MessageRecepientId == messageRecipientId);
            if (messageRecepient == null) return null;
            var cellPhoneNo = messageRecepient.Person.PersonOptionalFields.FirstOrDefault(o => o.OptionalFieldId == (int) OptionalFields.CellPhone);

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
                    Body = messageRecepient.Message.Body,
                    MessageToCellNo = cellPhoneNo == null ? string.Empty : cellPhoneNo.Value
                };
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