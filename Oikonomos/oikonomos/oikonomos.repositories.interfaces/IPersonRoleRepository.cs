using System.Collections.Generic;
using oikonomos.common.Models;
using oikonomos.data;

namespace oikonomos.repositories.interfaces
{
    public interface IPersonRoleRepository
    {
        void SavePersonChurchRole(PersonViewModel person, Person currentPerson, Person personToSave);
        IEnumerable<RoleViewModel> FetchSecurityRoles(Person currentPerson);
    }
}