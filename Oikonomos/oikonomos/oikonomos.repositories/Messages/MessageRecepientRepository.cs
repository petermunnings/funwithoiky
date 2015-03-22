using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
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

        public MessageQueueViewModel GetNextQueuedEmail()
        {
            var messageRecepient = Context.MessageRecepients.OrderBy(m=>m.MessageSent).FirstOrDefault(m=>m.Status == MessageStatus.Queued);
            if (messageRecepient == null) return null;

            var personChurch = Context.PersonChurches.FirstOrDefault(p => p.PersonId == messageRecepient.Message.MessageFrom);

            if (personChurch == null)
            {
                MarkMessageAsFailed(messageRecepient);
                return null;
            }

            var church = Context.Churches.FirstOrDefault(c => c.ChurchId == personChurch.ChurchId);
            if (church == null)
            {
                MarkMessageAsFailed(messageRecepient);
                return null;
            }

            //Get attachments
            var messageAttachments = Context.MessageAttachments.Where(m => m.MessageId == messageRecepient.MessageId).ToList();
            List<Attachment> attachments = null;
            if (messageAttachments.Any())
            {
                attachments = messageAttachments.Select(attachment => new Attachment(new MemoryStream(attachment.FileContent), attachment.FileName, attachment.FileType)).ToList();
            }
            
            return new MessageQueueViewModel
            {
                MessageId = messageRecepient.MessageId,
                MessageRecepientId = messageRecepient.MessageRecepientId,
                MessageToId = messageRecepient.MessageTo,
                MessageToEmail = messageRecepient.Person.Email,
                MessageToFullName = messageRecepient.Person.Firstname + " " + messageRecepient.Person.Family.FamilyName,
                MessageFromId = messageRecepient.Message.MessageFrom,
                Subject = messageRecepient.Message.Subject,
                Body = messageRecepient.Message.Body,
                ChurchId = church.ChurchId,
                EmailLogin = church.EmailLogin,
                EmailPassword = church.EmailPassword,
                Attachments = attachments
             };
        }

        private void MarkMessageAsFailed(MessageRecepient messageRecepient)
        {
            messageRecepient.Status = MessageStatus.Failed;
            messageRecepient.StatusMessage = "Could not find a valid church linked to the sender";
            Context.SaveChanges();
        }

        public void UpdateMessageRecepient(int messageRecepientId, string messageStatus, string statusDescription = "")
        {
            var messageRecepient = Context.MessageRecepients.FirstOrDefault(m => m.MessageRecepientId == messageRecepientId);
            if (messageRecepient == null) return;
            messageRecepient.Status = messageStatus;
            if (statusDescription != "")
                messageRecepient.StatusMessage = statusDescription;
            Context.SaveChanges();
        }
    }
}