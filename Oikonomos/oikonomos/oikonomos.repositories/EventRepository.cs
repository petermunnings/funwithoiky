using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AutoMapper;
using oikonomos.common;
using oikonomos.common.DTOs;
using oikonomos.common.Models;
using oikonomos.data;
using oikonomos.data.DataAccessors;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class EventRepository : RepositoryBase, IEventRepository
    {
        private readonly IBirthdayRepository _birthdayRepository;

        public EventRepository(IBirthdayRepository birthdayRepository)
        {
            _birthdayRepository = birthdayRepository;
            Mapper.CreateMap<Event, EventDto>();
            Mapper.CreateMap<EventDto, Event>();
        }

        EventDto IEventRepository.GetItem(int eventId)
        {
            var eventItem = Context.Events.First(e => e.EventId == eventId);
            return Mapper.Map<Event, EventDto>(eventItem);
        }

        int IEventRepository.SaveItem(EventDto eventDto)
        {
            var existingEvent = Context.Events.FirstOrDefault(e => e.ChurchId == eventDto.ChurchId && e.Name == eventDto.Name);
            if (existingEvent != null)
                return existingEvent.EventId;

            var newEvent = new Event();
            Mapper.Map(eventDto, newEvent);
            Context.AddToEvents(newEvent);
            Context.SaveChanges();
            return newEvent.EventId;
        }

        IEnumerable<EventDto> IEventRepository.GetListOfEventsForGroup(int churchId)
        {
            return ListOfEventsForGroup(churchId);
        }

        private IEnumerable<EventDto> ListOfEventsForGroup(int churchId)
        {
            return Context
                .Events
                .Where(e => e.ShowInGroupScreen && e.ChurchId == churchId)
                .OrderBy(e => e.EventOrder)
                .Select(e => new EventDto
                                 {
                                     Name = e.Name,
                                     ChurchId = churchId,
                                     EventId = e.EventId
                                 });
        }

        IEnumerable<PersonEventDto> IEventRepository.GetPersonEventsForGroup(int groupId, Person currentPerson)
        {
            if(!currentPerson.HasPermission(Permissions.ViewDiscipleship101)) return new List<PersonEventDto>();
            var group = Context.Groups.FirstOrDefault(g => g.GroupId == groupId);
            if (group == null)
                throw new ApplicationException("Invalid GroupId");

            var people = GroupDataAccessor.FetchPeopleInGroup(currentPerson, groupId);

            var events = ListOfEventsForGroup(group.ChurchId);
            return people.Select(person => new PersonEventDto
                                                       {
                                                           FullName = person.FullName, 
                                                           PersonId = person.PersonId, 
                                                           CompletedEvents = GetCompletedEvents(events, person.PersonId)
                                                       }).ToList();
            
        }

        private Dictionary<string, string> GetCompletedEvents(IEnumerable<EventDto> events, int personId )
        {
            var completedEvents = new Dictionary<string, string>();
            foreach(var personEvent in events)
            {
                var completedStatus = Context.PersonEvents.FirstOrDefault(pe => pe.EventId == personEvent.EventId && pe.PersonId == personId);
                var completed = "false";
                if(completedStatus!=null)
                    completed = completedStatus.Completed ? "true" : "false";
                completedEvents.Add(personEvent.EventId.ToString(), completed);
            }
            return completedEvents;
        }

        public void UpdatePersonEvent(int personId, int eventId, bool completed)
        {
            var personEvent = Context.PersonEvents.FirstOrDefault(p => p.PersonId == personId && p.EventId == eventId);
            if (personEvent == null)
            {
                personEvent = new PersonEvent {PersonId = personId, EventId = eventId};
                Context.AddToPersonEvents(personEvent);
            }
            personEvent.Completed = completed;
            Context.SaveChanges();
        }


        void IEventRepository.DeleteItem(int eventId)
        {
            var eventToDelete = Context.Events.FirstOrDefault(e => e.EventId == eventId);
            Context.DeleteObject(eventToDelete);
            Context.SaveChanges();
        }

        public EventDisplayModel FetchEventsToDisplay(Person currentPerson)
        {
            //TODO there must be a better way to do this - on the database
            var events = new EventDisplayModel();
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var upcomingChurchEvents = (from e in context.OldEvents
                                            join c in context.Churches
                                                on e.Reference equals c.ChurchId
                                            where e.EventDate >= DateTime.Today
                                            && e.TableId == (int)Tables.Church
                                            && c.ChurchId == currentPerson.ChurchId
                                            && (e.EventVisibilityId == (int)EventVisibilities.Church ||
                                                e.EventVisibilityId == (int)EventVisibilities.Public)
                                            select new EventListModel
                                            {
                                                EntityName = c.Name,
                                                Description = e.Description,
                                                Date = e.EventDate
                                            }).ToList();

                var upcomingBirthdays = _birthdayRepository.GetBirthdays(currentPerson);

                var upcomingAnniversaries = (from p in context.People
                                             join c in context.PersonChurches
                                                 on p.PersonId equals c.PersonId
                                             join permissions in context.PermissionRoles
                                                 on c.RoleId equals permissions.RoleId
                                             where p.Anniversary.HasValue
                                                   && c.ChurchId == currentPerson.ChurchId
                                                   && (permissions.PermissionId == (int)Permissions.ShowEvents)
                                             select new EventListModel
                                             {
                                                 EntityName = p.Firstname + " " + p.Family.FamilyName,
                                                 Description = "Anniversary",
                                                 Date = p.Anniversary.Value
                                             }).ToList();

                AddBirthdays(upcomingBirthdays);
                AddAnniversaries(upcomingAnniversaries);

                var upcomingEvents = new List<EventListModel>(upcomingChurchEvents);
                upcomingEvents.AddRange(upcomingBirthdays);
                upcomingEvents.AddRange(upcomingAnniversaries);

                events.UpcomingEvents = SortAndLimitTo20(upcomingEvents);
                events.PastEvents = null;
                return events;

            }
        }

        public IEnumerable<PersonViewModel> FetchBirthdays(int monthId, Person currentPerson, string[] selectedRolesString)
        {
            var selectedRoles = new List<int>();

            foreach (var r in selectedRolesString)
            {
                int roleId;
                if (int.TryParse(r, out roleId))
                {
                    selectedRoles.Add(roleId);
                }
            }

            return _birthdayRepository.GetBirthdayListForAMonth(currentPerson, monthId, selectedRoles);
        }

        private static void AddAnniversaries(IEnumerable<EventListModel> upcomingAnniversaries)
        {
            foreach (var ub in upcomingAnniversaries)
            {
                ub.Date = DateTime.Today > new DateTime(DateTime.Today.Year, ub.Date.Month, ub.Date.Day)
                              ? new DateTime(DateTime.Today.Year + 1, ub.Date.Month, ub.Date.Day)
                              : new DateTime(DateTime.Today.Year, ub.Date.Month, ub.Date.Day);
            }
        }

        private static void AddBirthdays(IEnumerable<EventListModel> upcomingBirthdays)
        {
            foreach (var ub in upcomingBirthdays)
            {
                if (ub.Date.Month == 2 && ub.Date.Day == 29)
                {
                    if (DateTime.Today >= new DateTime(DateTime.Today.Year, 3, 1))
                        ub.Date = DateTime.IsLeapYear(DateTime.Now.Year + 1)
                                      ? new DateTime(DateTime.Now.Year + 1, 2, 29)
                                      : new DateTime(DateTime.Now.Year + 1, 2, 28);
                    else
                        ub.Date = DateTime.IsLeapYear(DateTime.Now.Year)
                                      ? new DateTime(DateTime.Now.Year, 2, 29)
                                      : new DateTime(DateTime.Now.Year, 2, 28);
                }
                else
                    ub.Date = DateTime.Today > new DateTime(DateTime.Today.Year, ub.Date.Month, ub.Date.Day)
                                  ? new DateTime(DateTime.Today.Year + 1, ub.Date.Month, ub.Date.Day)
                                  : new DateTime(DateTime.Today.Year, ub.Date.Month, ub.Date.Day);
            }
        }

        private static List<EventListModel> SortAndLimitTo20(List<EventListModel> upcomingEvents)
        {
            upcomingEvents.Sort((e1, e2) => e1.Date.CompareTo(e2.Date));

            var upcomingEventsLimited = new List<EventListModel>();
            int[] counter = { 0 };
            foreach (var upcomingEvent in upcomingEvents.TakeWhile(upcomingEvent => counter[0] < 20))
            {
                upcomingEventsLimited.Add(upcomingEvent);
                counter[0]++;
            }
            return upcomingEventsLimited;
        }
    }
}
