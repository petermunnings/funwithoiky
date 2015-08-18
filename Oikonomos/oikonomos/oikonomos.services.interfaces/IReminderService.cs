using System.Collections.Generic;
using Lib.Web.Mvc.JQuery.JqGrid;
using oikonomos.common.DTOs;
using oikonomos.data;

namespace oikonomos.services.interfaces
{
    public interface IReminderService
    {
        JqGridData FetchPeopleToBeRemindedJQGrid(Person currentPerson, string reminderType, JqGridRequest request);
        void RemovePersonFromReminderList(Person currentPerson, string reminderType, int personId);
        void AddPersonToReminderList(Person currentPerson, string reminderType, string reminderFrequency, int personId);
        void UpdateReminderList(Person currentPerson, string reminderType, string reminderFrequency);
        IEnumerable<ReminderDto> GetListOfMonthlyReminders(string reminderType);
        IEnumerable<ReminderDto> GetListOfWeeklyReminders(string reminderType);
    }
}