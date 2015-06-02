using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lib.Web.Mvc.JQuery.JqGrid;
using oikonomos.common.DTOs;
using oikonomos.common.Models;
using oikonomos.data;
using oikonomos.repositories.interfaces;
using oikonomos.services.interfaces;

namespace oikonomos.services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEmailService _emailService;
        private readonly IBirthdayAndAnniversaryRepository _birthdayAndAnniversaryRepository;

        public EventService(
            IEventRepository eventRepository,
            IEmailService emailService,
            IBirthdayAndAnniversaryRepository birthdayAndAnniversaryRepository)
        {
            _eventRepository = eventRepository;
            _emailService = emailService;
            _birthdayAndAnniversaryRepository = birthdayAndAnniversaryRepository;
        }

        public int CreateEvent(EventDto eventDto)
        {
            throw new NotImplementedException();
        }

        public void UpdateEvent(EventDto eventDto)
        {
            throw new NotImplementedException();
        }

        IEnumerable<PersonEventDto> IEventService.GetListOfCompletedEvents(int personId)
        {
            return new List<PersonEventDto>();
        }

        IEnumerable<EventDto> IEventService.GetListEventsForGroup(int churchId)
        {
            return _eventRepository.GetListOfEventsForGroup(churchId);
        }

        public EventDto GetEvent(int eventId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PersonEventDto> GetPersonEventsForGroup(int groupId, Person currentPerson)
        {
            return _eventRepository.GetPersonEventsForGroup(groupId, currentPerson);
        }

        public void UpdatePersonEvent(int personId, int eventId, bool completed)
        {
            _eventRepository.UpdatePersonEvent(personId, eventId, completed);
        }

        public EventDisplayModel FetchEventsToDisplay(Person currentPerson)
        {
            return _eventRepository.FetchEventsToDisplay(currentPerson);
        }

        public JqGridData FetchBirthdayList(Person currentPerson, JqGridRequest request, int monthId, string[] selectedRoles)
        {
            IEnumerable<PersonViewModel> listOfBirthdays = new List<PersonViewModel>();
            try
            {
                listOfBirthdays = _birthdayAndAnniversaryRepository.GetBirthdayListForAMonth(currentPerson, monthId, selectedRoles);
            }
            catch (Exception ex)
            {
                _emailService.SendExceptionEmail(ex);
            }
            
            var totalRecords = listOfBirthdays.Count();

            switch (request.sidx)
            {
                case "Firstname":
                    {
                        listOfBirthdays = request.sord.ToLower() == "asc" ? listOfBirthdays.OrderBy(p => p.Firstname).Skip((request.page - 1) * request.rows).Take(request.rows) : listOfBirthdays.OrderByDescending(p => p.Firstname).Skip((request.page - 1) * request.rows).Take(request.rows);
                        break;
                    }
                case "Surname":
                    {
                        listOfBirthdays = request.sord.ToLower() == "asc" ? listOfBirthdays.OrderBy(p => p.Surname).Skip((request.page - 1) * request.rows).Take(request.rows) : listOfBirthdays.OrderByDescending(p => p.Surname).Skip((request.page - 1) * request.rows).Take(request.rows);
                        break;
                    }
                case "Day":
                    {
                        listOfBirthdays = request.sord.ToLower() == "asc" ? listOfBirthdays.OrderBy(p => p.DateOfBirth_Value.Value.Day).Skip((request.page - 1) * request.rows).Take(request.rows) : listOfBirthdays.OrderByDescending(p => p.DateOfBirth_Value.Value.Day).Skip((request.page - 1) * request.rows).Take(request.rows);
                        break;
                    }
                case "MemberStatus":
                    {
                        listOfBirthdays = request.sord.ToLower() == "asc" ? listOfBirthdays.OrderBy(p => p.RoleName).ThenBy(p => p.DateOfBirth_Value.Value.Day).Skip((request.page - 1) * request.rows).Take(request.rows) : listOfBirthdays.OrderByDescending(p => p.RoleName).ThenBy(p => p.DateOfBirth_Value.Value.Day).Skip((request.page - 1) * request.rows).Take(request.rows);
                        break;
                    }
                case "Email":
                    {
                        listOfBirthdays = request.sord.ToLower() == "asc" ? listOfBirthdays.OrderBy(p => p.Email).Skip((request.page - 1) * request.rows).Take(request.rows) : listOfBirthdays.OrderByDescending(p => p.Email).Skip((request.page - 1) * request.rows).Take(request.rows);
                        break;
                    }
            }

            var peopleGridData = new JqGridData()
            {
                total = (int)Math.Ceiling((float)totalRecords / request.rows),
                page = request.page,
                records = totalRecords,
                rows = (from p in listOfBirthdays.AsEnumerable()
                        select new JqGridRow()
                        {
                            id = p.PersonId.ToString(),
                            cell = new string[] {
                                                        p.PersonId.ToString(),
                                                        p.DateOfBirth_Value.Value.Day.ToString(CultureInfo.InvariantCulture),
                                                        p.Firstname,
                                                        p.Surname,
                                                        p.RoleName,
                                                        p.HomePhone,
                                                        p.CellPhone,
                                                        p.Email
                                    }
                        }).ToArray()
            };

            return peopleGridData;
        }

        public JqGridData FetchAnniversaryList(Person currentPerson, JqGridRequest request, int monthId, string[] selectedRoles)
        {
            IEnumerable<PersonViewModel> listOfAnniversaries = new List<PersonViewModel>();
            try
            {
                listOfAnniversaries = _birthdayAndAnniversaryRepository.GetAnniversaryListForAMonth(currentPerson, monthId, selectedRoles);
            }
            catch (Exception ex)
            {
                _emailService.SendExceptionEmail(ex);
            }

            var totalRecords = listOfAnniversaries.Count();

            switch (request.sidx)
            {
                case "Firstname":
                    {
                        listOfAnniversaries = request.sord.ToLower() == "asc" ? listOfAnniversaries.OrderBy(p => p.Firstname).Skip((request.page - 1) * request.rows).Take(request.rows) : listOfAnniversaries.OrderByDescending(p => p.Firstname).Skip((request.page - 1) * request.rows).Take(request.rows);
                        break;
                    }
                case "Surname":
                    {
                        listOfAnniversaries = request.sord.ToLower() == "asc" ? listOfAnniversaries.OrderBy(p => p.Surname).Skip((request.page - 1) * request.rows).Take(request.rows) : listOfAnniversaries.OrderByDescending(p => p.Surname).Skip((request.page - 1) * request.rows).Take(request.rows);
                        break;
                    }
                case "Day":
                    {
                        listOfAnniversaries = request.sord.ToLower() == "asc" ? listOfAnniversaries.OrderBy(p => p.Anniversary_Value.Value.Day).Skip((request.page - 1) * request.rows).Take(request.rows) : listOfAnniversaries.OrderByDescending(p => p.Anniversary_Value.Value.Day).Skip((request.page - 1) * request.rows).Take(request.rows);
                        break;
                    }
                case "MemberStatus":
                    {
                        listOfAnniversaries = request.sord.ToLower() == "asc" ? listOfAnniversaries.OrderBy(p => p.RoleName).ThenBy(p => p.Anniversary_Value.Value.Day).Skip((request.page - 1) * request.rows).Take(request.rows) : listOfAnniversaries.OrderByDescending(p => p.RoleName).ThenBy(p => p.Anniversary_Value.Value.Day).Skip((request.page - 1) * request.rows).Take(request.rows);
                        break;
                    }
                case "Email":
                    {
                        listOfAnniversaries = request.sord.ToLower() == "asc" ? listOfAnniversaries.OrderBy(p => p.Email).Skip((request.page - 1) * request.rows).Take(request.rows) : listOfAnniversaries.OrderByDescending(p => p.Email).Skip((request.page - 1) * request.rows).Take(request.rows);
                        break;
                    }
            }

            var peopleGridData = new JqGridData()
            {
                total = (int)Math.Ceiling((float)totalRecords / request.rows),
                page = request.page,
                records = totalRecords,
                rows = (from p in listOfAnniversaries.AsEnumerable()
                        select new JqGridRow()
                        {
                            id = p.PersonId.ToString(),
                            cell = new string[] {
                                                        p.PersonId.ToString(),
                                                        p.Anniversary_Value.Value.Day.ToString(CultureInfo.InvariantCulture),
                                                        p.Firstname,
                                                        p.Surname,
                                                        p.RoleName,
                                                        p.HomePhone,
                                                        p.CellPhone,
                                                        p.Email
                                    }
                        }).ToArray()
            };

            return peopleGridData;
        }
    }
}
