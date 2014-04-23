using System.Collections.Generic;

namespace oikonomos.repositories.interfaces.Messages
{
    public interface IMessageRepository
    {
        int SaveMessage(int fromPersonId, string subject, string body, string messageType);
        
    }
}