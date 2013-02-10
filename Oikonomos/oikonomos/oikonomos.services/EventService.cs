using System;
using System.Collections.Generic;
using oikonomos.common.DTOs;
using oikonomos.data;
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

        IEnumerable<PersonEventDto> IEventService.GetListOfCompletedEvents(int personId)
        {
            return new List<PersonEventDto>();
        }

        IEnumerable<EventDto> IEventService.GetListEventsForGroup(int churchId)
        {
            return _eventRepository.GetListOfEventsForGroup(churchId);
        }

        public EventDto GetEvent(int eventId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PersonEventDto> GetPersonEventsForGroup(int groupId, Person currentPerson)
        {
            return _eventRepository.GetPersonEventsForGroup(groupId, currentPerson);
        }

        public void UpdatePersonEvent(int personId, int eventId, bool completed, Person currentPerson)
        {
            _eventRepository.UpdatePersonEvent(personId, eventId, completed, currentPerson);
        }
    }
}
