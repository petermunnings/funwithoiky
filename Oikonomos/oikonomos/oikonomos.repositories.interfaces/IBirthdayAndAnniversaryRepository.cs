using System.Collections.Generic;
using oikonomos.common.Models;
using oikonomos.data;

namespace oikonomos.repositories.interfaces
{
    public interface IBirthdayAndAnniversaryRepository
    {
        IList<EventListModel> GetBirthdays(Person currentPerson);
        IEnumerable<PersonViewModel> GetBirthdayListForAMonth(Person currentPerson, int monthId, IEnumerable<string> selectedRolesString);
        IEnumerable<PersonViewModel> GetAnniversaryListForAMonth(Person currentPerson, int monthId, IEnumerable<string> selectedRolesString);
    }
}