using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using oikonomos.common.DTOs;
using oikonomos.data;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly oikonomosEntities _context;

        public EventRepository(oikonomosEntities context)
        {
            _context = context;
            Mapper.CreateMap<Event, EventDto>();
            Mapper.CreateMap<EventDto, Event>();
        }

        public IEnumerable<EventDto> GetListOfCompletedEvents(int personId)
        {
            throw new System.NotImplementedException();
        }

        EventDto IEventRepository.GetItem(int eventId)
        {
            var eventItem = _context.Events.First(e => e.EventId == eventId);
            return Mapper.Map<Event, EventDto>(eventItem);
        }

        int IEventRepository.SaveItem(EventDto eventDto)
        {
            var existingEvent = _context.Events.FirstOrDefault(e => e.ChurchId == eventDto.ChurchId && e.Name == eventDto.Name);
            if (existingEvent != null)
                return existingEvent.EventId;

            var newEvent = new Event();
            Mapper.Map(eventDto, newEvent);
            _context.AddToEvents(newEvent);
            _context.SaveChanges();
            return newEvent.EventId;
        }

        void IEventRepository.DeleteItem(int eventId)
        {
            var eventToDelete = _context.Events.FirstOrDefault(e => e.EventId == eventId);
            _context.DeleteObject(eventToDelete);
            _context.SaveChanges();
        }
    }
}
