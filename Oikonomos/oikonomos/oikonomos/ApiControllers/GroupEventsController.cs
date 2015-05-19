using System.Collections.Generic;
using System.Web.Http;
using oikonomos.common.DTOs;

namespace oikonomos.web.ApiControllers
{
    public class GroupEventsController : ApiController
    {
        public IEnumerable<PersonEventDto> Post([FromBody]int groupId)
        {
            return new List<PersonEventDto>();
        }

        public IEnumerable<PersonEventDto> Get(string id)
        {
            return new List<PersonEventDto>();
        }
    }
}