using System;
using System.Collections.Generic;

namespace oikonomos.common.Models
{
    public class EventDisplayModel
    {
        public List<EventListModel> UpcomingEvents { get; set; }
        public List<EventListModel> PastEvents { get; set; }
        public int SelectedChurchId { get; set; } 
    }

    public class EventListModel
    {
        public string EntityName { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string DateDisplay
        {
            get { return Date.ToString("dd MMM"); }
        }
    }
}
