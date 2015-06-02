using System.Collections.Generic;
using System.Linq;
using oikonomos.common;
using oikonomos.common.Models;
using oikonomos.data;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class BirthdayAndAniversaryRepository : RepositoryBase, IBirthdayAndAnniversaryRepository
    {
        public IList<EventListModel> GetBirthdays(Person currentPerson)
        {
            return (from p in Context.People
                join c in Context.PersonChurches
                    on p.PersonId equals c.PersonId
                join permissions in Context.PermissionRoles
                    on c.RoleId equals permissions.RoleId
                where p.DateOfBirth.HasValue
                      && c.ChurchId == currentPerson.ChurchId
                      && (permissions.PermissionId == (int)Permissions.ShowEvents)
                select new EventListModel
                {
                    EntityName = p.Firstname + " " + p.Family.FamilyName,
                    Description = "Birthday",
                    Date = p.DateOfBirth.Value
                }).ToList();
        }

        public IEnumerable<PersonViewModel> GetAnniversaryListForAMonth(Person currentPerson, int monthId, IEnumerable<string> selectedRolesString)
        {
            var selectedRoles = GetSelectedRoles(selectedRolesString);
            var list = Context.PersonChurches.Where(pc => pc.ChurchId == currentPerson.ChurchId && pc.Person.Anniversary != null && selectedRoles.Contains(pc.RoleId)).ToList();
            return (from l in list
                    where l.Person != null && (l.Person.Anniversary.HasValue && l.Person.Anniversary.Value.Month == monthId)
                    let cellPhone = l.Person.PersonOptionalFields.FirstOrDefault(cp => cp.OptionalFieldId == (int)OptionalFields.CellPhone)
                    select new PersonViewModel
                    {
                        PersonId = l.PersonId,
                        DateOfBirth_Value = l.Person.DateOfBirth,
                        Anniversary_Value = l.Person.Anniversary,
                        CellPhone = cellPhone == null ? string.Empty : cellPhone.Value,
                        HomePhone = l.Person.Family.HomePhone,
                        Email = l.Person.Email,
                        Firstname = l.Person.Firstname,
                        Surname = l.Person.Family.FamilyName,
                        RoleName = l.Role.Name
                    }).ToList();
        }

        public IEnumerable<PersonViewModel> GetBirthdayListForAMonth(Person currentPerson, int monthId, IEnumerable<string> selectedRolesString)
        {
            var selectedRoles = GetSelectedRoles(selectedRolesString);
            var list = Context.PersonChurches.Where(pc => pc.ChurchId == currentPerson.ChurchId && pc.Person.DateOfBirth !=null && selectedRoles.Contains(pc.RoleId)).ToList();
            return (from l in list
                where l.Person != null && (l.Person.DateOfBirth.HasValue && l.Person.DateOfBirth.Value.Month == monthId)
                let cellPhone = l.Person.PersonOptionalFields.FirstOrDefault(cp => cp.OptionalFieldId == (int) OptionalFields.CellPhone)
                select new PersonViewModel
                {
                    PersonId = l.PersonId, 
                    DateOfBirth_Value = l.Person.DateOfBirth, 
                    Anniversary_Value = l.Person.Anniversary,
                    CellPhone = cellPhone == null ? string.Empty : cellPhone.Value, 
                    HomePhone = l.Person.Family.HomePhone,
                    Email = l.Person.Email, 
                    Firstname = l.Person.Firstname, 
                    Surname = l.Person.Family.FamilyName,
                    RoleName = l.Role.Name
                }).ToList();

        }

        private static List<int> GetSelectedRoles(IEnumerable<string> selectedRolesString)
        {
            var selectedRoles = new List<int>();

            foreach (var r in selectedRolesString)
            {
                int roleId;
                if (int.TryParse(r, out roleId))
                {
                    selectedRoles.Add(roleId);
                }
            }
            return selectedRoles;
        }
    }
}