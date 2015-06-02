using System.Collections.Generic;
using oikonomos.common;
using oikonomos.common.Models;
using oikonomos.data;

namespace oikonomos.repositories.interfaces
{
    public interface IPermissionRepository
    {
        bool CheckSavePermissionPersonal(PersonViewModel person, Person currentPerson);
        bool CheckSavePermissionGroup(PersonViewModel person, Person currentPerson);
        void SetupPermissions(Person currentPerson, bool sysAdmin);
        void SetupPermissions(Person currentPerson, Church church, bool sysAdmin);
        IEnumerable<Person> FetchPeopleWithASpecificPermission(Permissions permission, int churchId);
    }
}