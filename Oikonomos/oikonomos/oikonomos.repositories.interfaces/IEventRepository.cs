using System.Collections.Generic;
using oikonomos.common.DTOs;

namespace oikonomos.repositories.interfaces
{
    public interface IEventRepository
    {
        IEnumerable<EventDto> GetListOfCompletedEvents(int personId);
        EventDto              GetItem(int eventId);
        void                  DeleteItem(int eventId);
        int                   SaveItem(EventDto eventDto);
        IEnumerable<EventDto> GetListOfEventsForGroup(int churchId);
    }
}