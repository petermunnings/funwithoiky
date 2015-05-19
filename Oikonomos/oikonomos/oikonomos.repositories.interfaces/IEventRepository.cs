using System.Collections.Generic;
using oikonomos.common.DTOs;
using oikonomos.common.Models;
using oikonomos.data;

namespace oikonomos.repositories.interfaces
{
    public interface IEventRepository
    {
        EventDto              GetItem(int eventId);
        void                  DeleteItem(int eventId);
        int                   SaveItem(EventDto eventDto);
        IEnumerable<EventDto> GetListOfEventsForGroup(int churchId);
        IEnumerable<PersonEventDto> GetPersonEventsForGroup(int groupId, Person currentPerson);
        void UpdatePersonEvent(int personId, int eventId, bool completed);
        EventDisplayModel FetchEventsToDisplay(Person currentPerson);
        IEnumerable<PersonViewModel> FetchBirthdays(int monthId, Person currentPerson, string[] selectedRolesString);
    }
}