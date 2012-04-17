using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using oikonomos.data;
using oikonomos.common;
using System.Configuration;
using oikonomos.common.Models;
using System.Data.Objects.DataClasses;
using System.Web.Security;
using System.Data.Objects;
using Lib.Web.Mvc.JQuery.JqGrid;
using LinqKit;

namespace oikonomos.data.DataAccessors
{
    public static class EventDataAccessor
    {
        public static JqGridData FetchEventListJQGrid(Person currentPerson, int personId, JqGridRequest request)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                int visibilityLevel = 2;
                if (currentPerson.HasPermission(Permissions.ViewPersonalComments))
                {
                    visibilityLevel = 1;
                }

                var events = (from e in context.Events
                              where e.Reference == personId
                                  && e.TableId == (int)Tables.Person
                                  && e.EventVisibilityId >= visibilityLevel
                              select e);

                int totalRecords = events.Count();

                switch (request.sidx)
                {
                    case "Date":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                events = events.OrderBy(e => e.EventDate).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                events = events.OrderByDescending(e => e.EventDate).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "Event":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                events = events.OrderBy(e => e.Description).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                events = events.OrderByDescending(e => e.Description).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "CreatedBy":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                events = events.OrderBy(e => e.CreatedByPerson.Firstname).ThenBy(e => e.CreatedByPerson.Family.FamilyName).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                events = events.OrderByDescending(e => e.CreatedByPerson.Firstname).ThenBy(e => e.CreatedByPerson.Family.FamilyName).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                }

                JqGridData eventsGridData = new JqGridData()
                {
                    total = (int)Math.Ceiling((float)totalRecords / (float)request.rows),
                    page = request.page,
                    records = totalRecords,
                    rows = (from e in events.AsEnumerable()
                            select new JqGridRow()
                            {
                                id = e.EventId.ToString(),
                                cell = new string[] {
                                                    e.Reference.ToString(),
                                                    e.EventDate.ToString("dd MMM yyyy"),
                                                    e.Description=="Comment" ? e.Comments : e.Description,
                                                    e.CreatedByPerson.Firstname + " " + e.CreatedByPerson.Family.FamilyName,
                                                    e.Comments
                                }
                            }).ToArray()
                };
                return eventsGridData;
            }
        }
        
        public static JqGridData FetchEventListJQGrid(Person currentPerson, DateTime fromDate, DateTime toDate, JqGridRequest request)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                int visibilityLevel = (int)EventVisibilities.Elders;

                toDate = toDate.AddDays(1);

                int churchId = currentPerson.ChurchId;

                var predicate = PredicateBuilder.True<Event>();
                var datePredicate = PredicateBuilder.True<Event>();
                var descriptionPredicate = PredicateBuilder.False<Event>();

                var events = (from e in context.Events
                              select e);
                
                datePredicate = datePredicate.And(e => e.TableId == (int)Tables.Person
                                  && e.EventVisibilityId >= visibilityLevel
                                  && e.EventDate >= fromDate
                                  && e.EventDate < toDate
                                  && (!e.Description.StartsWith("Attended Group"))
                                  && (!e.Description.StartsWith("Did not attend Group"))
                                  && e.Description != "Comment"
                                  && e.ChurchId == churchId);

                predicate = predicate.And(datePredicate.Expand());

                if (request._search)
                {
                    switch (request.searchField)
                    {
                        case "search":
                            {
                                string[] predicateFields = request.searchString.Split(',');

                                for (int count = 0; count < predicateFields.Length; count++)
                                {
                                    string temp = predicateFields[count].Trim();
                                    descriptionPredicate = descriptionPredicate.Or(p => p.Description.Contains(temp));
                                }
 
                                predicate = predicate.And(descriptionPredicate.Expand());
                                    
                                break;
                            }
                    }
                }

                events = context.Events.AsExpandable().Where(predicate);

                int totalRecords = events.Count();

                switch (request.sidx)
                {
                    case "Date":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                events = events.OrderBy(e => e.EventDate).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                events = events.OrderByDescending(e => e.EventDate).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "Event":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                events = events.OrderBy(e => e.Description).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                events = events.OrderByDescending(e => e.Description).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "CreatedBy":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                events = events.OrderBy(e => e.CreatedByPerson.Firstname).ThenBy(e => e.CreatedByPerson.Family.FamilyName).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                events = events.OrderByDescending(e => e.CreatedByPerson.Firstname).ThenBy(e => e.CreatedByPerson.Family.FamilyName).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                }

                JqGridData eventsGridData = new JqGridData()
                {
                    total = (int)Math.Ceiling((float)totalRecords / (float)request.rows),
                    page = request.page,
                    records = totalRecords,
                    rows = (from e in events.AsEnumerable()
                            select new JqGridRow()
                            {
                                id = e.EventId.ToString(),
                                cell = new string[] {
                                                    e.Reference.ToString(),
                                                    string.Empty,
                                                    e.EventDate.ToString("dd MMM yyyy"),
                                                    e.Description=="Comment" ? e.Comments : e.Description,
                                                    e.CreatedByPerson.Firstname + " " + e.CreatedByPerson.Family.FamilyName,
                                                    e.Comments
                                }
                            }).ToArray()
                };

                foreach (JqGridRow row in eventsGridData.rows)
                {
                    int personId = int.Parse(row.cell[0]);
                    var person = (from p in context.People.Include("Family") where p.PersonId == personId select p).FirstOrDefault();
                    row.cell[1] = person==null ? string.Empty : person.Firstname + ' ' + person.Family.FamilyName;
                }
                return eventsGridData;
            }
        }
       
        public static List<AttendanceEventViewModel> FetchGroupAttendance(Person currentPerson, int groupId, DateTime eventDate)
        {
            return FetchGroupAttendance(currentPerson, groupId, eventDate, null);
        }

        public static void SavePersonComment(int personId, string comment, Person currentPerson)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                Event commentEvent = new Event();
                commentEvent.Description = EventNames.Comment;
                commentEvent.Comments = comment;
                commentEvent.EventVisibilityId = (int)EventVisibilities.GroupAdministrators;
                commentEvent.CreatedByPersonId = currentPerson.PersonId;
                commentEvent.ChangedByPersonId = currentPerson.PersonId;
                commentEvent.Created = DateTime.Now;
                commentEvent.Changed = DateTime.Now;
                commentEvent.EventDate = DateTime.Now;
                commentEvent.TableId = (int)Tables.Person;
                commentEvent.Reference = personId;
                commentEvent.ChurchId = currentPerson.ChurchId;

                context.AddToEvents(commentEvent);

                context.SaveChanges();
            }
        }

        public static List<EventListModel> FetchCommentHistory(int personId, Person currentPerson)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                return (from e in context.Events
                        where e.TableId == (int)Tables.Person
                        && e.Reference == personId
                        && e.CreatedByPersonId == currentPerson.PersonId
                        && e.Description == EventNames.Comment
                        orderby e.EventDate descending
                        select new EventListModel
                        {
                            EntityName = e.Description,
                            Description = e.Comments,
                            Date = e.EventDate
                        }).Take(5).ToList();
            }
        }

        public static Dictionary<int, string> FetchGroupComments(Person currentPerson, int groupId)
        {
            Dictionary<int, string> comments = new Dictionary<int, string>();
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                ObjectResult<FetchComments_Result> results = context.FetchComments(groupId);
                foreach (FetchComments_Result result in results)
                {
                    comments.Add(result.Reference, result.Comments);
                }
                return comments;
            }
        }

        public static List<AttendanceEventViewModel> FetchGroupAttendance(Person currentPerson, int groupId, DateTime startDate, DateTime? endDate)
        {
            List<AttendanceEventViewModel> groupAttendance = new List<AttendanceEventViewModel>();
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                if (!(currentPerson.HasPermission(Permissions.EditOwnGroups) || currentPerson.HasPermission(Permissions.EditAllGroups)))
                {
                    throw new Exception("Invalid security Role");
                }

                if (!(currentPerson.HasPermission(Permissions.EditAllGroups)) && currentPerson.HasPermission(Permissions.EditOwnGroups))
                {
                    var gr = (from g in context.Groups
                             where (g.LeaderId == currentPerson.PersonId || g.AdministratorId == currentPerson.PersonId)
                             && g.GroupId == groupId
                             select g).FirstOrDefault();
                    if (gr == null)
                    {
                        throw new Exception("Invalid security Role");
                    }
                }

                string groupIdString = groupId.ToString();
                var attendanceList = (from e in context.Events
                                      join pg in context.PersonGroups
                                         on e.Reference equals pg.PersonId
                                      where (e.Description.StartsWith(EventNames.AttendedGroup) || e.Description.StartsWith(EventNames.DidNotAttendGroup))
                                      && e.TableId == (int)Tables.Person
                                      && pg.GroupId == groupId
                                      && e.Value == groupIdString
                                      orderby e.EventDate, pg.Person.Family.FamilyName, pg.Person.Created
                                      select new AttendanceEventViewModel
                                      {
                                          PersonId = pg.PersonId,
                                          FamilyId = pg.Person.FamilyId,
                                          Firstname = pg.Person.Firstname,
                                          Surname = pg.Person.Family.FamilyName,
                                          Attended = e.Description.StartsWith(EventNames.AttendedGroup),
                                          Date = e.EventDate,
                                          RoleId = (from pr in pg.Person.PersonRoles select pr.RoleId).FirstOrDefault(),
                                          Role = pg.Person.PersonRoles.FirstOrDefault().Role.Name
                                      });

                if (endDate == null)
                {
                    return (from ae in attendanceList
                            where ae.Date == startDate
                            select ae).ToList();
                }
                else
                {
                    return (from ae in attendanceList
                            where ae.Date >= startDate
                            && ae.Date <= endDate.Value
                            select ae).ToList();
                }
            }
        }

        public static EventDisplayModel FetchEventsToDisplay(Person currentPerson)
        {
            //TODO there must be a better way to do this - on the database
            EventDisplayModel events = new EventDisplayModel();
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var upcomingChurchEvents = (from e in context.Events
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

                var upcomingBirthdays = (from p in context.People
                                         where p.DateOfBirth.HasValue
                                         && p.ChurchId == currentPerson.ChurchId
                                         select new EventListModel
                                         {
                                             EntityName = p.Firstname + " " + p.Family.FamilyName,
                                             Description = "Birthday",
                                             Date = p.DateOfBirth.Value
                                         }).ToList();

                var upcomingAnniversaries = (from p in context.People
                                         where p.Anniversary.HasValue
                                         && p.ChurchId == currentPerson.ChurchId
                                         select new EventListModel
                                         {
                                             EntityName = p.Firstname + " " + p.Family.FamilyName,
                                             Description = "Anniversary",
                                             Date = p.Anniversary.Value
                                         }).ToList();

                //TODO - see if this can be done in Linq
                foreach (EventListModel ub in upcomingBirthdays)
                {
                    if (ub.Date.Month == 2 && ub.Date.Day == 29)
                    {
                        if (DateTime.Today >= new DateTime(DateTime.Today.Year, 3, 1))
                        {
                            if (DateTime.IsLeapYear(DateTime.Now.Year + 1))
                            {
                                ub.Date = new DateTime(DateTime.Now.Year + 1, 2, 29);
                            }
                            else
                            {
                                ub.Date = new DateTime(DateTime.Now.Year + 1, 2, 28);
                            }
                        }
                        else
                        {
                            if (DateTime.IsLeapYear(DateTime.Now.Year))
                            {
                                ub.Date = new DateTime(DateTime.Now.Year, 2, 29);
                            }
                            else
                            {
                                ub.Date = new DateTime(DateTime.Now.Year, 2, 28);
                            }
                        }
                    }
                    else
                    {
                        if (DateTime.Today > new DateTime(DateTime.Today.Year, ub.Date.Month, ub.Date.Day))
                        {
                            ub.Date = new DateTime(DateTime.Today.Year + 1, ub.Date.Month, ub.Date.Day);
                        }
                        else
                        {
                            ub.Date = new DateTime(DateTime.Today.Year, ub.Date.Month, ub.Date.Day);
                        }
                    }
                }

                foreach (EventListModel ub in upcomingAnniversaries)
                {
                    if (DateTime.Today > new DateTime(DateTime.Today.Year, ub.Date.Month, ub.Date.Day))
                    {
                        ub.Date = new DateTime(DateTime.Today.Year + 1, ub.Date.Month, ub.Date.Day);
                    }
                    else
                    {
                        ub.Date = new DateTime(DateTime.Today.Year, ub.Date.Month, ub.Date.Day);
                    }
                }

                List<EventListModel> upcomingEvents = new List<EventListModel>(upcomingChurchEvents);
                upcomingEvents.AddRange(upcomingBirthdays);
                upcomingEvents.AddRange(upcomingAnniversaries);

                //Sort and limit to 20

                upcomingEvents.Sort(delegate(EventListModel e1, EventListModel e2)
                {
                    return e1.Date.CompareTo(e2.Date);
                });

                List<EventListModel> upcomingEventsLimited = new List<EventListModel>();
                int counter = 0;
                foreach (EventListModel upcomingEvent in upcomingEvents)
                {
                    if (counter < 20)
                    {
                        upcomingEventsLimited.Add(upcomingEvent);
                        counter++;
                    }
                    else
                    {
                        break;
                    }
                }

                events.PastEvents = null;
                events.UpcomingEvents = upcomingEventsLimited;
                return events;

            }
        }

        public static void SaveHomeGroupEvent(Person currentPerson, HomeGroupEventViewModel hgEvent)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                int didAttend = 0;
                int didNotAttend = 0;
                foreach (PersonEventViewModel personEvents in hgEvent.Events)
                {
                    Person person = PersonDataAccessor.FetchPerson(personEvents.PersonId);
                    foreach (EventViewModel personEvent in personEvents.Events)
                    {
                        
                        Event pe = SavePersonEvent(context, personEvents, currentPerson, personEvent);
                        CheckToSeeIfEventAlreadyExists(personEvents, context, personEvent, pe);

                        if (person.HasPermission(Permissions.IncludeInGroupAttendanceStats))
                        {
                            if(personEvent.Name == EventNames.DidNotAttendGroup)
                                didNotAttend++;
                            if(personEvent.Name == EventNames.AttendedGroup)
                                didAttend++;
                        }
                    }
                }

                //Add the attended and did not attend group events
                AddAttendanceEvents(hgEvent, context, didAttend, "Members attended", currentPerson);
                AddAttendanceEvents(hgEvent, context, didNotAttend, "Members did not attend", currentPerson);

                context.SaveChanges();
            }
        }

        public static void SavePersonEvents(PersonEventViewModel personEvents, Person currentPerson)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                if (personEvents != null)
                {
                    foreach (EventViewModel personEvent in personEvents.Events)
                    {
                        Event pe = SavePersonEvent(context, personEvents, currentPerson, personEvent);
                        CheckToSeeIfEventAlreadyExists(personEvents, context, personEvent, pe);
                    }

                    context.SaveChanges();
                }
            }
        }

        #region Private Methods
        private static Event SavePersonEvent(oikonomosEntities context, PersonEventViewModel personEvents, Person currentPerson, EventViewModel personEvent)
        {
            Event pe = new Event();
            pe.Created = DateTime.Now;
            pe.CreatedByPersonId = currentPerson.PersonId;
            pe.Changed = DateTime.Now;
            pe.ChangedByPersonId = currentPerson.PersonId;
            pe.Description = personEvent.Name;
            pe.TableId = (int)Tables.Person;
            pe.EventVisibilityId = (int)EventVisibilities.GroupAdministrators;
            pe.Reference = personEvents.PersonId;
            pe.EventDate = personEvent.Date;
            pe.ChurchId = currentPerson.ChurchId;
            if (personEvent.GroupId != 0)
            {
                pe.Value = personEvent.GroupId.ToString();
                if (personEvent.Name == EventNames.AttendedGroup 
                    || personEvent.Name == EventNames.DidNotAttendGroup
                    || personEvent.Name == EventNames.LeftTheGroup)
                {
                    var hgName = (from g in context.Groups
                                  where g.GroupId == personEvent.GroupId
                                  select g.Name).FirstOrDefault();
                    pe.Description += string.Format(" ({0})", hgName);
                }
            }
            return pe;
        }

        private static void CheckToSeeIfEventAlreadyExists(PersonEventViewModel personEvents, oikonomosEntities context, EventViewModel personEvent, Event pe)
        {
            string groupId = personEvent.GroupId.ToString();
            //Check to see if this event already exists
            Event duplicateEvent = null;
            var check = (from e in context.Events
                         where e.TableId == (int)Tables.Person
                         && e.Reference == personEvents.PersonId
                         && e.EventDate == personEvent.Date
                         && e.Description.StartsWith(personEvent.Name) //Caters for the attended homegroup (blah)
                         select e);

            if (personEvent.GroupId != 0)
            {
                duplicateEvent = (from e in check
                                  where e.Value == groupId
                                  select e).FirstOrDefault();
            }
            else
            {
                duplicateEvent = (from e in check select e).FirstOrDefault();
            }

            if (duplicateEvent == null)
            {
                context.Events.AddObject(pe);
            }

            if (personEvent.Name == EventNames.AttendedGroup)
            {
                //Remove a "did not attend event on the same date"
                var atCheck = (from e in context.Events
                               where e.TableId == (int)Tables.Person
                               && e.Reference == personEvents.PersonId
                               && e.EventDate == personEvent.Date
                               && e.Description.StartsWith(EventNames.DidNotAttendGroup)
                               select e);
                
                if (personEvent.GroupId != 0)
                {
                    duplicateEvent = (from e in atCheck
                                      where e.Value == groupId
                                      select e).FirstOrDefault();
                }
                else
                {
                    duplicateEvent = (from e in check select e).FirstOrDefault();
                }

                if (duplicateEvent != null)
                {
                    context.DeleteObject(duplicateEvent);
                }
            }

            if (personEvent.Name == EventNames.DidNotAttendGroup)
            {
                //Remove an "attended" event on the same date
                var atCheck = (from e in context.Events
                               where e.TableId == (int)Tables.Person
                               && e.Reference == personEvents.PersonId
                               && e.EventDate == personEvent.Date
                               && e.Description.StartsWith(EventNames.AttendedGroup)
                               select e);
                
                if (personEvent.GroupId != 0)
                {
                    duplicateEvent = (from e in atCheck
                                      where e.Value == groupId
                                      select e).FirstOrDefault();
                }
                else
                {
                    duplicateEvent = (from e in check select e).FirstOrDefault();
                }

                if (duplicateEvent != null)
                {
                    context.DeleteObject(duplicateEvent);
                }
            }
        }

        private static void AddAttendanceEvents(HomeGroupEventViewModel hgEvent, oikonomosEntities context, int eventValue, string eventName, Person currentPerson)
        {
            Event groupEvent = new Event();
            groupEvent.Created = DateTime.Now;
            groupEvent.CreatedByPersonId = currentPerson.PersonId;
            groupEvent.ChurchId = currentPerson.ChurchId;

            //Check to see if it is not already in the database
            var groupCheck = (from e in context.Events
                              where e.TableId == (int)Tables.Group
                              && e.Reference == hgEvent.GroupId
                              && e.EventDate == hgEvent.EventDate
                              && e.Description == eventName
                              select e).FirstOrDefault();

            if (groupCheck != null)
            {
                groupEvent = groupCheck;
            }
            else
            {
                context.Events.AddObject(groupEvent);
            }

            groupEvent.TableId = (int)Tables.Group;
            groupEvent.Reference = hgEvent.GroupId;
            groupEvent.Description = eventName;
            groupEvent.EventDate = hgEvent.EventDate;
            groupEvent.Value = eventValue.ToString();
            groupEvent.EventVisibilityId = (int)EventVisibilities.GroupAdministrators;
            groupEvent.Changed = DateTime.Now;
            groupEvent.ChangedByPersonId = currentPerson.PersonId;

        }

        #endregion Private Methods
    }
}