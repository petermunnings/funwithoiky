using System.Collections.Generic;
using oikonomos.common.DTOs;

namespace oikonomos.services.interfaces
{
    public interface IEventService
    {
        int  CreateEvent(EventDto eventDto);
        void UpdateEvent(EventDto eventDto);

        IEnumerable<EventDto> GetListOfCompletedEvents(int personId);
    }
}