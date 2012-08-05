using oikonomos.common.DTOs;

namespace oikonomos.repositories.interfaces
{
    public interface IEventTypeRepository
    {
        EventTypeDto GetItem(int eventTypeId);
        int          SaveItem(EventTypeDto eventTypeDto);
        void         DeleteItem(int eventTypeId);
    }
}
