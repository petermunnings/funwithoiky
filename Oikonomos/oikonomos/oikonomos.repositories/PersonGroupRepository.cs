using System;
using System.Collections.Generic;
using System.Linq;
using oikonomos.common;
using oikonomos.common.Models;
using oikonomos.data;
using oikonomos.data.DataAccessors;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class PersonGroupRepository : RepositoryBase, IPersonGroupRepository
    {
        private readonly IPersonRepository _personRepository;

        public PersonGroupRepository(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public bool AddPersonToGroup(Person currentPerson, int personId, int groupId)
        {
            var personToSave = _personRepository.FetchPerson(personId);
            return AddPersonToGroup(groupId, currentPerson, personToSave);
        }
        
        public bool AddPersonToGroup(PersonViewModel person, Person currentPerson, Person personToSave)
        {
            return AddPersonToGroup(person.GroupId, currentPerson, personToSave);
        }

        private bool AddPersonToGroup(int groupId, Person currentPerson, Person personToSave)
        {
            if (groupId == 0) return false;
            
            var currentPersonGroups = personToSave.PersonGroups.Where(pg => pg.Group.ChurchId == currentPerson.ChurchId);
            if (currentPersonGroups.Any(g => g.GroupId == groupId))
            {
                return false;
            }

            personToSave.PersonGroups.Add(new PersonGroup { GroupId = groupId, Created = DateTime.Now, Changed = DateTime.Now, Joined = DateTime.Now });
            Context.SaveChanges();
            return true;
        }

        public IEnumerable<PersonGroup> GetPersonGroups(int personId, int churchId)
        {
            return Context.PersonGroups.Where(pg => pg.PersonId == personId && pg.Group.ChurchId == churchId);
        }

        public IEnumerable<PersonGroupViewModel> GetPersonGroupViewModels(int personId, Person currentPerson)
        {
            var allGroupsInChurch = Context.Groups.Where(g => g.ChurchId == currentPerson.ChurchId);
            var groupsPersonIsIn = GetPersonGroups(personId, currentPerson.ChurchId);
            var primaryGroup = GetPrimaryGroup(personId, currentPerson);

            var returnItems = PopulatePersonGroupViewModels(allGroupsInChurch, primaryGroup, groupsPersonIsIn);
            return returnItems;
        }

        public void RemovePersonFromGroup(Person currentPerson, int personId, int groupId)
        {
            if (!currentPerson.HasPermission(Permissions.RemovePersonFromGroup)) return;
            //Fetch the record
            var personToRemove = (from pg in Context.PersonGroups
                                  where pg.PersonId == personId
                                        && pg.GroupId == groupId
                                  select pg).FirstOrDefault();
            if (personToRemove == null) return;
            //Remove them
            Context.PersonGroups.DeleteObject(personToRemove);
            Context.SaveChanges();
        }

        private IEnumerable<PersonGroupViewModel> PopulatePersonGroupViewModels(IEnumerable<Group> allGroupsInChurch, Group primaryGroup, IEnumerable<PersonGroup> groupsPersonIsIn)
        {
            var returnItems = new List<PersonGroupViewModel>();
            foreach (var g in allGroupsInChurch)
            {
                var personGroupViewModel = new PersonGroupViewModel
                    {
                        GroupId = g.GroupId,
                        GroupName = g.Name,
                        IsPrimaryGroup = primaryGroup != null && g.GroupId == primaryGroup.GroupId
                    };
                var tgroup = g;
                foreach (var groupIsIn in groupsPersonIsIn.Where(groupIsIn => groupIsIn.GroupId == tgroup.GroupId))
                {
                    personGroupViewModel.IsInGroup = true;
                    break;
                }
                returnItems.Add(personGroupViewModel);
            }
            return returnItems;
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