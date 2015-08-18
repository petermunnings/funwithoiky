using System;
using System.Collections.Generic;
using System.Linq;
using Lib.Web.Mvc.JQuery.JqGrid;
using oikonomos.common.DTOs;
using oikonomos.data;
using oikonomos.repositories.interfaces;
using oikonomos.services.interfaces;

namespace oikonomos.services
{
    public class ReminderService : IReminderService
    {
        private readonly IReminderRepository _reminderRepository;

        public ReminderService(IReminderRepository reminderRepository)
        {
            _reminderRepository = reminderRepository;
        }

        public JqGridData FetchPeopleToBeRemindedJQGrid(Person currentPerson, string reminderType, JqGridRequest request)
        {
            var people = _reminderRepository.FetchPeopleToBeReminded(currentPerson.ChurchId, reminderType);
            var totalRecords = people.Count();
            var sort = "Surname";
            var sortList = request.sidx.Split(',');
            if (sortList.Count() > 1)
                sort = sortList[1].Trim();
            switch (sort)
            {
                case "Firstname":
                    {
                        people = request.sord.ToLower() == "asc" ? people.OrderBy(p => p.Firstname) : people.OrderByDescending(p => p.Firstname);
                        break;
                    }
                case "Surname":
                    {
                        people = request.sord.ToLower() == "asc" ? people.OrderBy(p => p.Surname) : people.OrderByDescending(p => p.Surname);
                        break;
                    }
            }

            if (request.rows > 0)
                people = people.Skip((request.page - 1) * request.rows).Take(request.rows);
            else
                request.page = 1;

            var peopleGridData = new JqGridData()
            {
                total = request.rows > 0 ? (int) Math.Ceiling((float) totalRecords/(float) request.rows) : 1,
                page = request.page,
                records = totalRecords,
                rows = (from p in people.AsEnumerable()
                    select new JqGridRow()
                    {
                        id = p.PersonId.ToString(),
                        cell = new[]
                        {
                            p.PersonId.ToString(),
                            p.Firstname,
                            p.Surname,
                            p.Email
                        }
                    }).ToArray()
            };

            return peopleGridData;
        }

        public void RemovePersonFromReminderList(Person currentPerson, string reminderType, int personId)
        {
            //Check if person has permission to do this
            _reminderRepository.RemovePersonFromReminderList(reminderType, personId, currentPerson.ChurchId);
        }

        public void AddPersonToReminderList(Person currentPerson, string reminderType, string reminderFrequency, int personId)
        {
            //Check if person has permission to do this
            _reminderRepository.AddPersonToReminderList(reminderType, reminderFrequency, personId, currentPerson.ChurchId);
        }

        public void UpdateReminderList(Person currentPerson, string reminderType, string reminderFrequency)
        {
            //Check if person has permission to do this
            _reminderRepository.UpdateReminderList(reminderType, reminderFrequency, currentPerson.ChurchId);
        }

        public IEnumerable<ReminderDto> GetListOfMonthlyReminders(string reminderType)
        {
            return _reminderRepository.GetListOfReminders(reminderType, "monthly");
        }

        public IEnumerable<ReminderDto> GetListOfWeeklyReminders(string reminderType)
        {
            return _reminderRepository.GetListOfReminders(reminderType, "weekly");
        }
    }
}