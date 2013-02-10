using oikonomos.common.Models;
using oikonomos.data;

namespace oikonomos.repositories.interfaces
{
    public interface IAddressRepository
    {
        void SaveAddressInformation(PersonViewModel person, Address address, Family family);
    }
}