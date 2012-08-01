using oikonomos.common.DTOs;

namespace oikonomos.services.interfaces
{
    public interface IEventTypeService
    {
        int Save(EventTypeDto newEventType);
        EventTypeDto GetItem(int eventTypeId);
    }
}