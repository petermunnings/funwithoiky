using System.Collections.Generic;
using oikonomos.common.DTOs;
using oikonomos.data;

namespace oikonomos.services.interfaces
{
    public interface IEventService
    {
        int  CreateEvent(EventDto eventDto);
        void UpdateEvent(EventDto eventDto);
        IEnumerable<PersonEventDto> GetListOfCompletedEvents(int personId);
        IEnumerable<EventDto> GetListEventsForGroup(int churchId);
        EventDto GetEvent(int eventId);
        IEnumerable<PersonEventDto> GetPersonEventsForGroup(int groupId, Person currentPerson);
        void UpdatePersonEvent(int personId, int eventId, bool completed, Person currentPerson);
    }
}