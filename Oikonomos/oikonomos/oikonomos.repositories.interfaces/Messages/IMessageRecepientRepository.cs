using System.Collections.Generic;
using oikonomos.common.Models;

namespace oikonomos.repositories.interfaces.Messages
{
    public interface IMessageRecepientRepository
    {
        MessageRecepientViewModel FetchMessageRecepient(int messageRecipientId);
        void SaveMessageRecepient(int messageId, IEnumerable<int> toPersonIds, string messageStatus, string errorMessage);
        MessageQueueViewModel GetNextQueuedEmail();
        int GetNoOfOutstandingMessages();
        void UpdateMessageRecepient(int messageRecepientId, string messageStatus, string statusDescription = "");
        IEnumerable<MessageWithStatusViewModel> FetchMessagesWithStatuses();
    }
}