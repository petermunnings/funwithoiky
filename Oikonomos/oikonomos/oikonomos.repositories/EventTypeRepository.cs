using System.Configuration;
using System.Linq;
using AutoMapper;
using oikonomos.common.DTOs;
using oikonomos.data;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class EventTypeRepository : IEventTypeRepository
    {
        public EventTypeRepository()
        {
            Mapper.CreateMap<EventType, EventTypeDto>();
        }

        public EventTypeDto GetItem(int eventTypeId)
        {
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var eventType = context.EventTypes.First(e => e.EventTypeId == eventTypeId);
                return Mapper.Map<EventType, EventTypeDto>(eventType);
            }
        }
    }
}
