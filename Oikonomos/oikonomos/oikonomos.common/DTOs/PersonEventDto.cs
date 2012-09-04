using System.Collections.Generic;

namespace oikonomos.common.DTOs
{
    public class PersonEventDto
    {
        public int PersonId { get; set; }
        public string FullName { get; set; }
        public IDictionary<int, bool> CompletedEvents { get; set; }
    }
}