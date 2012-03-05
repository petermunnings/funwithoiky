using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oikonomos.common.Models
{
    public class PersonEventViewModel
    {
        public int PersonId { get; set; }
        public bool IsVisitor { get; set; }
        public List<EventViewModel> Events { get; set; }

    }

    public class EventViewModel
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int GroupId { get; set; }
    }
}
