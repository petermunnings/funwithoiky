using System.Collections.Generic;

namespace oikonomos.repositories.interfaces
{
    public interface IMessageRepository
    {
        void SaveMessage(int fromPersonId, IEnumerable<int> toPeopleIds, string subject, string body, string messageType, string messageStatus);
    }
}