using oikonomos.common.Models;
using oikonomos.data;

namespace oikonomos.repositories.interfaces
{
    public interface IGroupEventRepository
    {
        void Save(Person currentPerson, HomeGroupEventViewModel hgEvent);
    }
}