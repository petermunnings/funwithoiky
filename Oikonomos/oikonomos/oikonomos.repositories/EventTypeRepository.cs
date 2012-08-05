using System.Configuration;
using System.Data.Objects;
using System.Linq;
using AutoMapper;
using oikonomos.common.DTOs;
using oikonomos.data;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class EventTypeRepository : IEventTypeRepository
    {
        private readonly oikonomosEntities _context;

        public EventTypeRepository(oikonomosEntities context)
        {
            _context = context;
            Mapper.CreateMap<EventType, EventTypeDto>();
            Mapper.CreateMap<EventTypeDto, EventType>();
        }

        EventTypeDto IEventTypeRepository.GetItem(int eventTypeId)
        {
            var eventType = _context.EventTypes.First(e => e.EventTypeId == eventTypeId);
            return Mapper.Map<EventType, EventTypeDto>(eventType);
        }

        int IEventTypeRepository.SaveItem(EventTypeDto eventTypeDto)
        {
            var existingEventType = _context.EventTypes.FirstOrDefault(e => e.ChurchId == eventTypeDto.ChurchId && e.Name == eventTypeDto.Name);
            if (existingEventType != null)
                return existingEventType.EventTypeId;

            var newEventType = new EventType();
            Mapper.Map(eventTypeDto, newEventType);
            _context.AddToEventTypes(newEventType);
            _context.SaveChanges();
            return newEventType.EventTypeId;
        }

        void IEventTypeRepository.DeleteItem(int eventTypeId)
        {
            var eventTypeToDelete = _context.EventTypes.FirstOrDefault(e => e.EventTypeId == eventTypeId);
            _context.DeleteObject(eventTypeToDelete);
            _context.SaveChanges();
        }
    }
}
