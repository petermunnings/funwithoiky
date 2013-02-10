using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using oikonomos.common;
using oikonomos.common.DTOs;
using oikonomos.data;
using oikonomos.data.DataAccessors;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class EventRepository : RepositoryBase, IEventRepository
    {
        public EventRepository()
        {
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

        public void UpdatePersonEvent(int personId, int eventId, bool completed, Person currentPerson)
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
    }
}
