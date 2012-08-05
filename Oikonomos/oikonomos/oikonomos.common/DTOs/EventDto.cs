using System;

namespace oikonomos.common.DTOs
{
    public class EventDto
    {
        public int       EventId     { get; set; }
        public int       EventTypeId { get; set; }
        public DateTime? EventDate   { get; set; }
        public string    Comments    { get; set; }
    }
}