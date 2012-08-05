using oikonomos.common.DTOs;

namespace oikonomos.services.interfaces
{
    public interface IEventTypeService
    {
        int          CreateEventType(EventTypeDto newEventType);
        EventTypeDto GetEventType(int eventTypeId);
        void         DeleteEventType(int eventTypeId);
    }
}