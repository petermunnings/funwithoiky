using System;
using System.Collections.Generic;

namespace oikonomos.common.DTOs
{
    [Serializable]
    public class PersonEventDto
    {
        public int PersonId { get; set; }
        public string FullName { get; set; }
        public IDictionary<string, string> CompletedEvents { get; set; }
    }
}