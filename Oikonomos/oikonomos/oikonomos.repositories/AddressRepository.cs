using System;
using oikonomos.common.Models;
using oikonomos.data;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class AddressRepository : RepositoryBase, IAddressRepository
    {
        public void SaveAddressInformation(PersonViewModel person, Address address, Family family)
        {
            if (address == null)
            {
                address = new Address { Created = DateTime.Now, Changed = DateTime.Now };
                family.Address = address;
            }

            if (address.Line1 != person.Address1 ||
                address.Line2 != person.Address2 ||
                address.Line3 != person.Address3 ||
                address.Line4 != person.Address4)
                address.Changed = DateTime.Now;

            address.Line1 = person.Address1 ?? string.Empty;
            address.Line2 = person.Address2 ?? string.Empty;
            address.Line3 = person.Address3 ?? string.Empty;
            address.Line4 = person.Address4 ?? string.Empty;
            address.Lat = person.Lat;
            address.Long = person.Lng;

            Context.SaveChanges();
        }
    }
}