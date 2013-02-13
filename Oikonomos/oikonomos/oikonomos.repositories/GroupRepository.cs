using System.Linq;
using oikonomos.common.Models;
using oikonomos.data;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class GroupRepository : RepositoryBase, IGroupRepository
    {
        public void PopulateGroupId(int personId, Person currentPerson, PersonViewModel personViewModel)
        {
            Group group = null;
            if (Context.PersonGroups.Count(pg => pg.PersonId == personId && pg.Group.ChurchId == currentPerson.ChurchId) == 1)
                group = (from pg in Context.PersonGroups
                         where pg.PersonId == personId && pg.Group.ChurchId == currentPerson.ChurchId
                         select pg.Group).FirstOrDefault();
            else
                group = (from pg in Context.PersonGroups
                         where pg.PersonId == personId && pg.Group.ChurchId == currentPerson.ChurchId && pg.PrimaryGroup
                         select pg.Group).FirstOrDefault()
                        ?? (from pg in Context.PersonGroups
                            where pg.PersonId == personId && pg.Group.ChurchId == currentPerson.ChurchId
                            select pg.Group).FirstOrDefault();

            personViewModel.GroupId = group == null ? 0 : group.GroupId;
            personViewModel.GroupName = group == null ? "None" : group.Name;

            personViewModel.IsInMultipleGroups = Context.PersonGroups.Count(pg => pg.PersonId == personId && pg.Group.ChurchId == currentPerson.ChurchId) > 1;
        }

        public Group GetGroup(int groupId)
        {
            return Context.Groups.FirstOrDefault(g => g.GroupId == groupId);
        }
    }
}