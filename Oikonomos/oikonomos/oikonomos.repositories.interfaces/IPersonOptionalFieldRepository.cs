using oikonomos.common.Models;
using oikonomos.data;

namespace oikonomos.repositories.interfaces
{
    public interface IPersonOptionalFieldRepository
    {
        void SaveOptionalFields(PersonViewModel person, Person personToSave);

    }
}