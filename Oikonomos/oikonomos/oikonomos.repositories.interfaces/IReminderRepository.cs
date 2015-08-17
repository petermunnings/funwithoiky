using System.Collections.Generic;
using oikonomos.common.DTOs;
using oikonomos.common.Models;

namespace oikonomos.repositories.interfaces
{
    public interface IReminderRepository
    {
        IEnumerable<PersonListViewModel> FetchPeopleToBeReminded(int churchId, string reminderType);
        void RemovePersonFromReminderList(string reminderType, int personId, int churchId);
        void AddPersonToReminderList(string reminderType, string reminderFrequency, int personId, int churchId);
        void UpdateReminderList(string reminderType, string reminderFrequency, int churchId);
        IEnumerable<ReminderDto> GetListOfReminders(string reminderType, string reminderFrequency);
    }
}