using oikonomos.common.DTOs;

namespace oikonomos.repositories.interfaces.Messages
{
    public interface IMessageRepository
    {
        int SaveMessage(int fromPersonId, string subject, string body, string messageType);
        PersonDto GetSender(int messageId);
    }
}