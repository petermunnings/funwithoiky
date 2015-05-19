using System.Collections.Generic;
using oikonomos.common.Models;
using oikonomos.data;

namespace oikonomos.repositories.interfaces
{
    public interface IBirthdayRepository
    {
        List<EventListModel> GetBirthdays(Person currentPerson);
        IEnumerable<PersonViewModel> GetBirthdayListForAMonth(Person currentPerson, int monthId, IEnumerable<int> selectedRoles);
    }
}