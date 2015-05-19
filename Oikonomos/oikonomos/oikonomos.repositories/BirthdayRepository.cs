using System.Collections.Generic;
using System.Linq;
using oikonomos.common;
using oikonomos.common.Models;
using oikonomos.data;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class BirthdayRepository : RepositoryBase, IBirthdayRepository
    {
        public List<EventListModel> GetBirthdays(Person currentPerson)
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

        public IEnumerable<PersonViewModel> GetBirthdayListForAMonth(Person currentPerson, int monthId, IEnumerable<int> selectedRoles)
        {
            var list = Context.PersonChurches.Where(pc => pc.ChurchId == currentPerson.ChurchId && pc.Person.DateOfBirth != null && selectedRoles.Contains(pc.RoleId)).ToList();
            return (from l in list
                where l.Person != null && l.Person.DateOfBirth.HasValue && l.Person.DateOfBirth.Value.Month == monthId
                let cellPhone = l.Person.PersonOptionalFields.FirstOrDefault(cp => cp.OptionalFieldId == (int) OptionalFields.CellPhone)
                select new PersonViewModel
                {
                    PersonId = l.PersonId, 
                    DateOfBirth_Value = l.Person.DateOfBirth, 
                    CellPhone = cellPhone == null ? string.Empty : cellPhone.Value, 
                    HomePhone = l.Person.Family.HomePhone,
                    Email = l.Person.Email, 
                    Firstname = l.Person.Firstname, 
                    Surname = l.Person.Family.FamilyName,
                    RoleName = l.Role.Name
                }).ToList();

        }
    }
}