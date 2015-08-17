using System.Collections.Generic;
using oikonomos.common.Models;
using oikonomos.data;

namespace oikonomos.repositories.interfaces
{
    public interface IBirthdayAndAnniversaryRepository
    {
        IList<EventListModel> GetBirthdays(Person currentPerson);
        IEnumerable<PersonViewModel> GetBirthdayListForAMonth(int monthId, IEnumerable<string> selectedRolesString, int churchId);
        IEnumerable<PersonViewModel> GetAnniversaryListForAMonth(int monthId, IEnumerable<string> selectedRolesString, int churchId);
        IEnumerable<PersonViewModel> GetBirthdayListForAMonth(int selectedMonth, int churchId);
        IEnumerable<PersonViewModel> GetAnniversaryListForAMonth(int selectedMonth, int churchId);
    }
}