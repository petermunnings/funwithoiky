using System.Collections.Generic;
using oikonomos.common.DTOs;

namespace oikonomos.repositories.interfaces
{
    public interface IEventRepository
    {
        IEnumerable<EventDto> GetListOfCompletedEvents(int personId);
    }
}