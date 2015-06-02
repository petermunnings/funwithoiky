using System.Collections.Generic;
using Lib.Web.Mvc.JQuery.JqGrid;
using oikonomos.common.DTOs;
using oikonomos.common.Models;
using oikonomos.data;

namespace oikonomos.services.interfaces
{
    public interface IEventService
    {
        int  CreateEvent(EventDto eventDto);
        void UpdateEvent(EventDto eventDto);
        IEnumerable<PersonEventDto> GetListOfCompletedEvents(int personId);
        IEnumerable<EventDto> GetListEventsForGroup(int churchId);
        EventDto GetEvent(int eventId);
        IEnumerable<PersonEventDto> GetPersonEventsForGroup(int groupId, Person currentPerson);
        void UpdatePersonEvent(int personId, int eventId, bool completed);
        EventDisplayModel FetchEventsToDisplay(Person currentPerson);
        JqGridData FetchBirthdayList(Person currentPerson, JqGridRequest request, int monthId, string[] selectedRoles);
        JqGridData FetchAnniversaryList(Person currentPerson, JqGridRequest request, int monthId, string[] selectedRoles);
    }
}