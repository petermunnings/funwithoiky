using System;
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

        public IEnumerable<PersonViewModel> GetAnniversaryListForAMonth(int monthId, IEnumerable<string> selectedRolesString, int churchId)
        {
            var selectedRoles = GetSelectedRoles(selectedRolesString);
            var list = Context.PersonChurches.Where(pc => pc.ChurchId == churchId && pc.Person.Anniversary != null && selectedRoles.Contains(pc.RoleId)).ToList();
            return GetPersonViewModelAnniversaryList(monthId, list);
        }

        public IEnumerable<PersonViewModel> GetBirthdayListForAMonth(int selectedMonth, int churchId)
        {
            var list = Context.PersonChurches.Where(pc => pc.ChurchId == churchId && pc.Person.DateOfBirth != null).ToList();
            return GeneratePersonViewModelBirthdayList(selectedMonth, list);
        }

        public IEnumerable<PersonViewModel> GetAnniversaryListForAMonth(int selectedMonth, int churchId)
        {
            var list = Context.PersonChurches.Where(pc => pc.ChurchId == churchId && pc.Person.Anniversary != null).ToList();
            return GetPersonViewModelAnniversaryList(selectedMonth, list);
        }

        public IEnumerable<PersonViewModel> GetAnniversaryListForADateRange(DateTime startDate, DateTime endDate, int churchId)
        {
            var list = Context.PersonChurches.Where(pc => pc.ChurchId == churchId && pc.Person.Anniversary != null).ToList();
            return GeneratePersonViewModelAnniversaryListFromDateRange(startDate, endDate, list);
        }

        public IEnumerable<PersonViewModel> GetBirthdayListForADateRange(DateTime startDate, DateTime endDate, int churchId)
        {
            var list = Context.PersonChurches.Where(pc => pc.ChurchId == churchId && pc.Person.DateOfBirth != null).ToList();
            return GeneratePersonViewModelBirthdayListFromDateRange(startDate, endDate, list);
        }

        public IEnumerable<PersonViewModel> GetBirthdayListForAMonth(int monthId, IEnumerable<string> selectedRolesString, int churchId)
        {
            var selectedRoles = GetSelectedRoles(selectedRolesString);
            var list = Context.PersonChurches.Where(pc => pc.ChurchId == churchId && pc.Person.DateOfBirth != null && selectedRoles.Contains(pc.RoleId)).ToList();
            return GeneratePersonViewModelBirthdayList(monthId, list);
        }

        private static IEnumerable<PersonViewModel> GeneratePersonViewModelBirthdayListFromDateRange(DateTime startDate, DateTime endDate, IEnumerable<PersonChurch> list)
        {
            IEnumerable<PersonChurch> listOfBirthdays;

            if (startDate.Month == endDate.Month)
            {
                var monthId = startDate.Month;
                listOfBirthdays = list.Where(
                    l =>
                        l.Person != null &&
                        (l.Person.DateOfBirth.HasValue && l.Person.DateOfBirth.Value.Month == monthId && l.Person.DateOfBirth.Value.Day >= startDate.Day && l.Person.DateOfBirth.Value.Day <= endDate.Day));
            }
            else
            {
                var startMonthId = startDate.Month;
                var endMonthId = endDate.Month;
                listOfBirthdays = list.Where(
                    l =>
                        l.Person != null &&
                        (l.Person.DateOfBirth.HasValue &&
                         (
                             (l.Person.DateOfBirth.Value.Month == startMonthId && l.Person.DateOfBirth.Value.Day >= startDate.Day) ||
                             (l.Person.DateOfBirth.Value.Month == endMonthId && l.Person.DateOfBirth.Value.Day <= endDate.Day)
                             )));

            }
            
            return (from l in listOfBirthdays
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

        private static IEnumerable<PersonViewModel> GeneratePersonViewModelAnniversaryListFromDateRange(DateTime startDate, DateTime endDate, IEnumerable<PersonChurch> list)
        {
            IEnumerable<PersonChurch> listOfAnniversaries;

            if (startDate.Month == endDate.Month)
            {
                var monthId = startDate.Month;
                listOfAnniversaries = list.Where(
                    l =>
                        l.Person != null &&
                        (l.Person.Anniversary.HasValue && l.Person.Anniversary.Value.Month == monthId && l.Person.Anniversary.Value.Day >= startDate.Day && l.Person.Anniversary.Value.Day <= endDate.Day));
            }
            else
            {
                var startMonthId = startDate.Month;
                var endMonthId = endDate.Month;
                listOfAnniversaries = list.Where(
                    l =>
                        l.Person != null &&
                        (l.Person.Anniversary.HasValue &&
                         (
                             (l.Person.Anniversary.Value.Month == startMonthId && l.Person.Anniversary.Value.Day >= startDate.Day) ||
                             (l.Person.Anniversary.Value.Month == endMonthId && l.Person.Anniversary.Value.Day <= endDate.Day)
                             )));

            }

            return (from l in listOfAnniversaries
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


        private static IEnumerable<PersonViewModel> GeneratePersonViewModelBirthdayList(int monthId, IEnumerable<PersonChurch> list)
        {
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

        private static IEnumerable<PersonViewModel> GetPersonViewModelAnniversaryList(int monthId, IEnumerable<PersonChurch> list)
        {
            return (from l in list
                where l.Person != null && (l.Person.Anniversary.HasValue && l.Person.Anniversary.Value.Month == monthId)
                let cellPhone =
                    l.Person.PersonOptionalFields.FirstOrDefault(cp => cp.OptionalFieldId == (int) OptionalFields.CellPhone)
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