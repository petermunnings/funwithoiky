using System.Collections.Generic;
using oikonomos.common.Models;
using oikonomos.data;

namespace oikonomos.repositories.interfaces
{
    public interface IPersonGroupRepository
    {
        void SavePrimaryGroup(int personId, int groupId, Person currentPerson);
        Group GetPrimaryGroup(int personId, Person currentPerson);
        bool AddPersonToGroup(PersonViewModel person, Person currentPerson, Person personToSave);
        IEnumerable<PersonGroup> GetPersonGroups(int personId, int churchId);
    }
}