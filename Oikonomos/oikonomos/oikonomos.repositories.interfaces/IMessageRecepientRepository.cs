using oikonomos.common.Models;
using oikonomos.data;

namespace oikonomos.repositories.interfaces
{
    public interface IMessageRecepientRepository
    {
        MessageRecepientViewModel FetchMessageRecepient(int messageRecipientId);
    }
}