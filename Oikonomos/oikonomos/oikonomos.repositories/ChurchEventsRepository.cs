using System.Collections.Generic;
using oikonomos.common.Models;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class ChurchEventsRepository : RepositoryBase, IChurchEventsRepository
    {
        public IEnumerable<ChurchEventViewModel> FetchChurchEvents(int churchId)
        {
            var churchEvents = new List<ChurchEventViewModel>
            {
                new ChurchEventViewModel
                {
                    EventId = 1,
                    EventDate = "1st January 2015",
                    EventHeading = "SCC New Year Party!!!",
                    EventDescription =
                        "<p>Come and join us for a celebration of the new year.  2016 is upon us.  Let's celebrate.</p><p><b>Venue: </b>107 Coleraine Drive, Riverclub</p>",
                    EventImage = "abcd.png"
                }

            };
            return churchEvents;
        }
    }
}