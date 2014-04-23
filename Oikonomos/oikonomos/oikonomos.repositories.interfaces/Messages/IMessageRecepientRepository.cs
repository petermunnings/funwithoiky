using System.Collections.Generic;
using oikonomos.common.Models;

namespace oikonomos.repositories.interfaces.Messages
{
    public interface IMessageRecepientRepository
    {
        MessageRecepientViewModel FetchMessageRecepient(int messageRecipientId);
        void SaveMessageRecepient(int messageId, IEnumerable<int> toPersonIds, string messageStatus, string errorMessage);
    }
}