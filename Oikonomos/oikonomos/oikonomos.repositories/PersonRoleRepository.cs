using System.Collections.Generic;
using System.Linq;
using oikonomos.common.Models;
using oikonomos.data;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class PersonRoleRepository : RepositoryBase, IPersonRoleRepository
    {
        public void SavePersonChurchRole(PersonViewModel person, Person currentPerson, Person personToSave)
        {
            if (person.RoleId == 0)
                return;

            var personRole = Context.PersonChurches.FirstOrDefault(pr => (pr.Role.ChurchId == currentPerson.ChurchId) && (pr.PersonId == personToSave.PersonId));
            if (personRole == null)
            {
                SavePersonRole(personToSave, person.RoleId);
                return;
            }

            if (personRole.RoleId == person.RoleId)
                return;

            Context.DeleteObject(personRole);
            SavePersonRole(personToSave, person.RoleId);
        }

        public IEnumerable<RoleViewModel> FetchSecurityRoles(Person currentPerson)
        {
            List<RoleViewModel> roles;
            if (currentPerson.HasPermission(common.Permissions.SystemAdministrator))
                roles = (from r in Context.Roles
                         where r.ChurchId == currentPerson.ChurchId
                         select new RoleViewModel { RoleId = r.RoleId, Name = r.Name }).ToList();
            else
                roles = (from r in Context.Roles
                         from canSetRole in r.CanBeSetByRoles
                         where r.ChurchId == currentPerson.ChurchId
                               && canSetRole.RoleId == currentPerson.RoleId
                         select new RoleViewModel { RoleId = r.RoleId, Name = r.Name }).ToList();
            return roles;
        }

        private void SavePersonRole(Person personToSave, int roleId)
        {
            var pr = new PersonChurch { RoleId = roleId, ChurchId = personToSave.ChurchId, PersonId = personToSave.PersonId };
            Context.AddToPersonChurches(pr);
            Context.SaveChanges();
        }
    }
}