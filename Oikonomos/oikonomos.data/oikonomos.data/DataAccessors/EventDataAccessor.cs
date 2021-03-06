﻿using System;
using System.Collections.Generic;
using System.Linq;
using oikonomos.common;
using System.Configuration;
using oikonomos.common.Models;
using Lib.Web.Mvc.JQuery.JqGrid;
using LinqKit;

namespace oikonomos.data.DataAccessors
{
    public static class EventDataAccessor
    {
        public static JqGridData FetchEventListJQGrid(Person currentPerson, int personId, JqGridRequest request)
        {
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                int visibilityLevel = 2;
                if (currentPerson.HasPermission(Permissions.ViewComments))
                {
                    visibilityLevel = 1;
                }

                var events = (from e in context.OldEvents
                              where e.Reference == personId
                                  && e.TableId == (int)Tables.Person
                                  && e.EventVisibilityId >= visibilityLevel
                                  && e.ChurchId == currentPerson.ChurchId
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

                var predicate = PredicateBuilder.True<OldEvent>();
                var datePredicate = PredicateBuilder.True<OldEvent>();
                var descriptionPredicate = PredicateBuilder.False<OldEvent>();

                var events = (from e in context.OldEvents
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

                events = context.OldEvents.AsExpandable().Where(predicate);

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

                var eventsGridData = new JqGridData()
                {
                    total = (int)Math.Ceiling(totalRecords / (float)request.rows),
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
                OldEvent commentEvent = new OldEvent();
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

                context.AddToOldEvents(commentEvent);

                context.SaveChanges();
            }
        }

        public static Dictionary<int, string> FetchGroupComments(Person currentPerson, int groupId)
        {
            var comments = new Dictionary<int, string>();
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var includedRoles = context
                    .PermissionRoles
                    .Where(pr => pr.PermissionId == (int) Permissions.IncludeInGroupAttendanceStats)
                    .Select(pr => pr.RoleId)
                    .ToList();

                var peopleInGroup = (from pg in context.PersonGroups
                                     join pc in context.PersonChurches
                                         on pg.PersonId equals pc.PersonId
                                     where pg.GroupId == groupId
                                           && includedRoles.Contains(pc.RoleId)
                                           && pc.ChurchId == currentPerson.ChurchId
                                     select pg.PersonId);

                foreach(var personId in peopleInGroup)
                {
                    var mostRecentComment = context
                        .Comments
                        .Where(c => c.AboutPersonId == personId)
                        .OrderBy(c => c.CommentDate)
                        .Select(c=>c)
                        .FirstOrDefault();
                    if(mostRecentComment!=null)
                        comments.Add(mostRecentComment.AboutPersonId, mostRecentComment.Comment1);
                }

                return comments;
            }
        }

        public static List<AttendanceEventViewModel> FetchGroupAttendance(Person currentPerson, int groupId, DateTime startDate, DateTime? endDate)
        {
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
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

                var groupIdString = groupId.ToString();
                var attendanceList = (from pg in context.PersonGroups
                                      join e in context.OldEvents
                                         on pg.PersonId equals e.Reference 
                                      join pc in context.PersonChurches
                                         on pg.PersonId equals pc.PersonId
                                      where (e.Description.StartsWith(EventNames.AttendedGroup) || e.Description.StartsWith(EventNames.DidNotAttendGroup))
                                      && e.TableId == (int)Tables.Person
                                      && pg.GroupId == groupId
                                      && e.Value == groupIdString
                                      orderby e.EventDate, pg.Person.Family.FamilyName, pg.Person.Created
                                      select new AttendanceEventViewModel
                                      {
                                          PersonId  = pg.PersonId,
                                          FamilyId  = pg.Person.FamilyId,
                                          Firstname = pg.Person.Firstname,
                                          Surname   = pg.Person.Family.FamilyName,
                                          Attended  = e.Description.StartsWith(EventNames.AttendedGroup),
                                          Date      = e.EventDate,
                                          RoleId    = pc.RoleId,
                                          Role      = pc.Role.DisplayName
                                      });

                if (endDate == null)
                    return (from ae in attendanceList
                            where ae.Date == startDate
                            select ae).ToList();

                return (from ae in attendanceList
                        where ae.Date >= startDate
                              && ae.Date <= endDate.Value
                        select ae).ToList();
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
                        if (personEvent.Date == DateTime.MinValue)
                            personEvent.Date = DateTime.Now;
                        var pe = SavePersonEvent(context, personEvents, currentPerson, personEvent);
                        CheckToSeeIfEventAlreadyExists(personEvents, context, personEvent, pe);
                    }

                    context.SaveChanges();
                }
            }
        }

        #region Private Methods

        public static OldEvent SavePersonEvent(oikonomosEntities context, PersonEventViewModel personEvents, Person currentPerson, EventViewModel personEvent)
        {
            var pe = new OldEvent
                         {
                             Created = DateTime.Now,
                             CreatedByPersonId = currentPerson.PersonId,
                             Changed = DateTime.Now,
                             ChangedByPersonId = currentPerson.PersonId,
                             Description = personEvent.Name,
                             TableId = (int) Tables.Person,
                             EventVisibilityId = (int) EventVisibilities.GroupAdministrators,
                             Reference = personEvents.PersonId,
                             EventDate = personEvent.Date,
                             ChurchId = currentPerson.ChurchId
                         };
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

        public static void CheckToSeeIfEventAlreadyExists(PersonEventViewModel personEvents, oikonomosEntities context, EventViewModel personEvent, OldEvent pe)
        {
            string groupId = personEvent.GroupId.ToString();
            //Check to see if this event already exists
            OldEvent duplicateEvent = null;
            var check = (from e in context.OldEvents
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
                context.OldEvents.AddObject(pe);
            }

            if (personEvent.Name == EventNames.AttendedGroup)
            {
                //Remove a "did not attend event on the same date"
                var atCheck = (from e in context.OldEvents
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
                var atCheck = (from e in context.OldEvents
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

        public static void AddAttendanceEvents(HomeGroupEventViewModel hgEvent, oikonomosEntities context, int eventValue, string eventName, Person currentPerson)
        {
            OldEvent groupEvent = new OldEvent();
            groupEvent.Created = DateTime.Now;
            groupEvent.CreatedByPersonId = currentPerson.PersonId;
            groupEvent.ChurchId = currentPerson.ChurchId;

            //Check to see if it is not already in the database
            var groupCheck = (from e in context.OldEvents
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
                context.OldEvents.AddObject(groupEvent);
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