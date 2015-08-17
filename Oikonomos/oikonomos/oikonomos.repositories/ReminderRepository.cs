using System.Collections.Generic;
using System.Linq;
using oikonomos.common.DTOs;
using oikonomos.common.Models;
using oikonomos.data;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class ReminderRepository : RepositoryBase, IReminderRepository
    {
        IEnumerable<PersonListViewModel> IReminderRepository.FetchPeopleToBeReminded(int churchId, string reminderType)
        {
            var reminder = Context.Reminders.FirstOrDefault(r => r.ChurchId == churchId && r.ReminderType == reminderType);

            var returnList = new List<PersonListViewModel>();
            if (reminder == null) return returnList;
            foreach (var p in reminder.People)
            {
                returnList.Add(new PersonListViewModel
                {
                    PersonId = p.PersonId,
                    Surname = p.Family.FamilyName,
                    Firstname = p.Firstname,
                    Email = p.Email
                });
            }
            return returnList;

        }

        public void RemovePersonFromReminderList(string reminderType, int personId, int churchId)
        {
            var reminder = Context.Reminders.FirstOrDefault(r => r.ChurchId == churchId && r.ReminderType == reminderType);
            if (reminder == null) return;
            var personToBeRemoved = reminder.People.FirstOrDefault(p => p.PersonId == personId);
            if (personToBeRemoved == null) return;
            reminder.People.Remove(personToBeRemoved);
            Context.SaveChanges();
        }

        public void AddPersonToReminderList(string reminderType, string reminderFrequency, int personId, int churchId)
        {
            var reminder = Context.Reminders.FirstOrDefault(r => r.ChurchId == churchId && r.ReminderType == reminderType);
            if (reminder == null)
            {
                reminder = new Reminder
                {
                    ChurchId = churchId,
                    ReminderType = reminderType,
                    ReminderFrequency = reminderFrequency
                };

                Context.Reminders.AddObject(reminder);
            }
            var personToBeAdded = Context.People.FirstOrDefault(p => p.PersonId == personId);
            if (personToBeAdded == null) return;
            if (personToBeAdded.PersonChurches.Any(pc => pc.ChurchId == churchId))
            {
                reminder.People.Add(personToBeAdded);
                Context.SaveChanges();
            }
        }

        public void UpdateReminderList(string reminderType, string reminderFrequency, int churchId)
        {
            var reminder = Context.Reminders.FirstOrDefault(r => r.ChurchId == churchId && r.ReminderType == reminderType);
            if (reminder == null)
            {
                reminder = new Reminder
                {
                    ChurchId = churchId,
                    ReminderType = reminderType,
                    ReminderFrequency = reminderFrequency
                };

                Context.Reminders.AddObject(reminder);
            }
            reminder.ReminderFrequency = reminderFrequency;
            Context.SaveChanges();
        }

        public IEnumerable<ReminderDto> GetListOfReminders(string reminderType, string reminderFrequency)
        {
            var reminders = Context.Reminders.Where(r => r.ReminderType == reminderType && r.ReminderFrequency == reminderFrequency).ToList();
            var listOfMonthlyReminders = new List<ReminderDto>();
            foreach (var r in reminders)
            {
                listOfMonthlyReminders.AddRange(from p in r.People
                    where p.HasValidEmail()
                    select new ReminderDto { ChurchId = r.ChurchId, Email = p.Email, PersonId = p.PersonId});
            }
            return listOfMonthlyReminders;
        }
    }
}