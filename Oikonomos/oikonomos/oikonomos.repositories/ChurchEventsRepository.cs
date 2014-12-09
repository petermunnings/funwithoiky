using System.Collections.Generic;
using System.Linq;
using oikonomos.common.Models;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class ChurchEventsRepository : RepositoryBase, IChurchEventsRepository
    {
        public IEnumerable<ChurchEventViewModel> FetchChurchEvents(int churchId)
        {
            return Context.ChurchEvents.Where(c => c.ChurchId == churchId).ToList().Select(ce => new ChurchEventViewModel
            {
                EventId = ce.ChurchEventId, 
                EventDate = ce.EventDate.ToString("dd MMMM yyyy"), 
                EventHeading = ce.EventHeading, 
                EventDescription = ce.EventDescription, 
                EventImage = ce.EventImage
            }).ToList();
        }
    }
}