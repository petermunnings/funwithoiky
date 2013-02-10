using System.Collections.Generic;
using System.Linq;
using oikonomos.common.Models;
using oikonomos.data;
using oikonomos.data.DataAccessors;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class PersonGroupRepository : RepositoryBase, IPersonGroupRepository
    {
        public bool AddPersonToGroup(PersonViewModel person, Person currentPerson, Person personToSave)
        {
            var noGroupsInThisChurch = personToSave.PersonGroups.Count(pg => pg.Group.ChurchId == currentPerson.ChurchId);
            if (noGroupsInThisChurch > 1)
            {
                if (person.GroupId > 0)
                    SavePrimaryGroup(person.PersonId, person.GroupId, currentPerson);
                return false;
            }

            if (personToSave.PersonGroups.Count == 1 && personToSave.PersonGroups.First().GroupId != person.GroupId)
            {
                if (person.GroupId == 0)
                {
                    Context.DeleteObject(personToSave.PersonGroups.First());
                    return false;
                }

                personToSave.PersonGroups.First().GroupId = person.GroupId;
                return true;
            }

            if (personToSave.PersonGroups.Count == 0 && person.GroupId > 0)
            {
                PersonDataAccessor.SavePersonGroup(personToSave, person.GroupId);
                return true;
            }
            return false;
        }

        public IEnumerable<PersonGroup> GetPersonGroups(int personId, int churchId)
        {
            return Context.PersonGroups.Where(pg => pg.PersonId == personId && pg.Group.ChurchId == churchId);
        }

        public void SavePrimaryGroup(int personId, int groupId, Person currentPerson)
        {
            var personGroup = Context.PersonGroups.FirstOrDefault(pg => pg.PersonId == personId && pg.GroupId == groupId);
            if (personGroup == null) return;
            var allGroups = Context.PersonGroups.Where(pg => pg.PersonId == personId && pg.Group.ChurchId == currentPerson.ChurchId).ToList();
            foreach (var pg in allGroups)
            {
                pg.PrimaryGroup = pg.GroupId == groupId;
            }
            Context.SaveChanges();
        }

        public Group GetPrimaryGroup(int personId, Person currentPerson)
        {
            var personGroup = Context.PersonGroups.FirstOrDefault(pg => pg.PersonId == personId && pg.PrimaryGroup && pg.Group.ChurchId == currentPerson.ChurchId) ??
                              Context.PersonGroups.FirstOrDefault(pg => pg.PersonId == personId && pg.Group.ChurchId == currentPerson.ChurchId);
            return personGroup != null ? personGroup.Group : null;
        }
    }
}