using oikonomos.common.DTOs;
using oikonomos.repositories.interfaces;
using oikonomos.services.interfaces;

namespace oikonomos.services
{
    public class EventTypeService : IEventTypeService
    {
        private readonly IEventTypeRepository _eventTypeRepository;

        public EventTypeService(IEventTypeRepository eventTypeRepository)
        {
            _eventTypeRepository = eventTypeRepository;
        }

        int IEventTypeService.Save(EventTypeDto newEventType)
        {
            return 1;
        }

        public EventTypeDto GetItem(int eventTypeId)
        {
            return _eventTypeRepository.GetItem(eventTypeId);
        }
    }
}