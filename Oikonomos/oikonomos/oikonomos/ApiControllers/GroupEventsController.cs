using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;
using oikonomos.common.DTOs;
using oikonomos.data;
using oikonomos.repositories;
using oikonomos.services;
using oikonomos.services.interfaces;

namespace oikonomos.web.ApiControllers
{
    public class GroupEventsController : ApiController
    {
        private readonly oikonomosEntities _context;
        
        public GroupEventsController()
        {
            _context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString);
        }

        public IEnumerable<PersonEventDto> Post([FromBody]int groupId)
        {
            IEventService eventService = new EventService(new EventRepository());
            return new List<PersonEventDto>();
        }

        public IEnumerable<PersonEventDto> Get(string id)
        {
            var groupId = int.Parse(id);
            IEventService eventService = new EventService(new EventRepository());
            return new List<PersonEventDto>();
        }
    }
}