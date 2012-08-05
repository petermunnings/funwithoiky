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

        int IEventTypeService.CreateEventType(EventTypeDto newEventType)
        {
            return _eventTypeRepository.SaveItem(newEventType);
        }

        EventTypeDto IEventTypeService.GetEventType(int eventTypeId)
        {
            return _eventTypeRepository.GetItem(eventTypeId);
        }

        void IEventTypeService.DeleteEventType(int eventTypeId)
        {
            _eventTypeRepository.DeleteItem(eventTypeId);
        }
    }
}