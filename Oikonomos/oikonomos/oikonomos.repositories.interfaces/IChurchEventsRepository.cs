using System.Collections.Generic;
using oikonomos.common.Models;

namespace oikonomos.repositories.interfaces
{
    public interface IChurchEventsRepository
    {
        IEnumerable<ChurchEventViewModel> FetchChurchEvents(int churchId);
    }
}
