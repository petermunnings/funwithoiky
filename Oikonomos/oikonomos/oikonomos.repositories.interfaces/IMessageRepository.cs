using System.Collections.Generic;

namespace oikonomos.repositories.interfaces
{
    public interface IMessageRepository
    {
        int SaveMessage(int fromPersonId, string subject, string body, string messageType);
        void SaveMessageRecepient(int messageId, IEnumerable<int> toPersonIds, string messageStatus, string errorMessage);
    }
}