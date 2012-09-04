using System.Collections.Generic;
using System.Web.Http;
using oikonomos.common.DTOs;

namespace oikonomos.web.ApiControllers
{
    public class GroupEventsController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<PersonEventDto> Get()
        {
            return new List<PersonEventDto>{ new PersonEventDto{PersonId=1, FullName = "Peter Munnings", CompletedEvents = new Dictionary<int, bool>{{4,true},{5,true},{6,false},{7,true}} }};
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public IEnumerable<PersonEventDto> Post([FromBody]int groupId)
        {
            return new List<PersonEventDto> { new PersonEventDto { PersonId = 1, FullName = "Peter Munnings", CompletedEvents = new Dictionary<int, bool> { { 4, true }, { 5, true }, { 6, false }, { 7, true } } } };
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
            var c = "test";
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            var c = "test";
        }
    }
}