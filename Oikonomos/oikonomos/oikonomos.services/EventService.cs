using System;
using System.Collections.Generic;
using oikonomos.common.DTOs;
using oikonomos.repositories.interfaces;
using oikonomos.services.interfaces;

namespace oikonomos.services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public int CreateEvent(EventDto eventDto)
        {
            throw new NotImplementedException();
        }

        public void UpdateEvent(EventDto eventDto)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EventDto> GetListOfCompletedEvents(int personId)
        {
            return _eventRepository.GetListOfCompletedEvents(personId);
        }

        public EventDto GetEvent(int eventId)
        {
            throw new NotImplementedException();
        }
    }
}
