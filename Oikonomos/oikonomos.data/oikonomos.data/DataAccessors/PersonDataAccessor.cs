using System;
using System.Collections.Generic;
using System.Linq;
using oikonomos.common;
using System.Configuration;
using oikonomos.common.Models;
using oikonomos.data.Services;
using Lib.Web.Mvc.JQuery.JqGrid;

namespace oikonomos.data.DataAccessors
{
    public static class PersonDataAccessor
    {
        public static JqGridData FetchMessagesForPersonJQGrid(Person currentPerson, int personId, JqGridRequest request)
        {
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                IEnumerable<PersonMessageModel> messages = (from m in context.Messages
                                                        join mr in context.MessageRecepients
                                                          on m.MessageId equals mr.MessageId
                                                        join pc in context.PersonChurches
                                                            on mr.MessageTo equals pc.PersonId
                                                        where pc.PersonId == personId
                                                        && pc.ChurchId == currentPerson.ChurchId
                                                          select new PersonMessageModel
                                                        {
                                                            PersonId = pc.PersonId,
                                                            MessageId = mr.MessageRecepientId,
                                                            Subject = m.Subject,
                                                            Body = m.Body,
                                                            Sent = mr.MessageSent,
                                                            MessageType = m.MessageType,
                                                            Status = mr.Status,
                                                            StatusDetail = mr.StatusMessage,
                                                            SentBy = m.Person.Firstname + " " + m.Person.Family.FamilyName
                                                        })
                                                 .ToList();

                int totalRecords = messages.Count();

                switch (request.sidx)
                {
                    case "Subject":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                messages = messages.OrderBy(g => g.Subject)
                                    .Skip((request.page - 1) * request.rows)
                                    .Take(request.rows)
                                    .ToList();
                            }
                            else
                            {
                                messages = messages
                                    .OrderByDescending(g => g.Subject)
                                    .Skip((request.page - 1) * request.rows)
                                    .Take(request.rows)
                                    .ToList();
                            }
                            break;
                        }
                    case "Sent":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                messages = messages.OrderBy(g => g.Sent)
                                    .Skip((request.page - 1) * request.rows)
                                    .Take(request.rows)
                                    .ToList();
                            }
                            else
                            {
                                messages = messages
                                    .OrderByDescending(g => g.Sent)
                                    .Skip((request.page - 1) * request.rows)
                                    .Take(request.rows)
                                    .ToList();
                            }
                            break;
                        }
                    case "Status":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                messages = messages.OrderBy(g => g.Status)
                                    .Skip((request.page - 1) * request.rows)
                                    .Take(request.rows)
                                    .ToList();
                            }
                            else
                            {
                                messages = messages
                                    .OrderByDescending(g => g.Status)
                                    .Skip((request.page - 1) * request.rows)
                                    .Take(request.rows)
                                    .ToList();
                            }
                            break;
                        }
                    case "MessageType":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                messages = messages.OrderBy(g => g.MessageType)
                                    .Skip((request.page - 1) * request.rows)
                                    .Take(request.rows)
                                    .ToList();
                            }
                            else
                            {
                                messages = messages
                                    .OrderByDescending(g => g.MessageType)
                                    .Skip((request.page - 1) * request.rows)
                                    .Take(request.rows)
                                    .ToList();
                            }
                            break;
                        }
                    case "SentBy":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                messages = messages.OrderBy(g => g.SentBy)
                                    .Skip((request.page - 1) * request.rows)
                                    .Take(request.rows)
                                    .ToList();
                            }
                            else
                            {
                                messages = messages
                                    .OrderByDescending(g => g.SentBy)
                                    .Skip((request.page - 1) * request.rows)
                                    .Take(request.rows)
                                    .ToList();
                            }
                            break;
                        }
                }

                var messagesGridData = new JqGridData()
                {
                    total = (int)Math.Ceiling((float)totalRecords / (float)request.rows),
                    page = request.page,
                    records = totalRecords,
                    rows = (from g in messages
                            select new JqGridRow()
                            {
                                id = g.MessageId.ToString(),
                                cell = new string[] {
                                                    g.MessageId.ToString(),
                                                    g.Subject,
                                                    g.Sent.ToString("yyyy/MM/dd"),
                                                    g.Status,
                                                    g.MessageType,
                                                    g.SentBy
                                }
                            }).ToArray()
                };
                return messagesGridData;
            }
        }

        public static JqGridData FetchGroupsForPersonJQGrid(Person currentPerson, int personId, JqGridRequest request)
        {
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                IEnumerable<PersonGroupModel> groups = (from g in context.Groups
                                                 join pg in context.PersonGroups
                                                   on g.GroupId equals pg.GroupId
                                                 where pg.PersonId == personId
                                                     && g.ChurchId == currentPerson.ChurchId
                                                 select new PersonGroupModel
                                                 {
                                                     PersonId = pg.PersonId,
                                                     GroupId = g.GroupId,
                                                     Name = g.Name,
                                                     Type = g.GroupType.Name,
                                                     Administrator = g.Administrator.Firstname + " " + g.Administrator.Family.FamilyName,
                                                     Leader = g.Leader.Firstname + " " + g.Leader.Family.FamilyName,
                                                     PrimaryGroup = pg.PrimaryGroup
                                                 })
                                                 .ToList();

                foreach (PersonGroupModel pg in groups)
                {
                    string groupIdAsString = pg.GroupId.ToString();
                    DateTime lastDateAttended = (from e in context.OldEvents
                                            where e.Reference == personId
                                              && e.Value == groupIdAsString
                                            orderby e.EventDate descending
                                            select e.EventDate)
                                            .FirstOrDefault();

                    pg.LastAttended = lastDateAttended == DateTime.MinValue ? "Never" : lastDateAttended.ToString("dd MMM yyyy");

                    var groupClassification = context.Groups.FirstOrDefault(g => g.GroupId == pg.GroupId).GroupClassification;
                    if (groupClassification != null)
                        pg.Type = groupClassification.Name;
                }

                int totalRecords = groups.Count();

                switch (request.sidx)
                {
                    case "Name":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                groups = groups.OrderBy(g => g.Name)
                                    .Skip((request.page - 1) * request.rows)
                                    .Take(request.rows)
                                    .ToList();
                            }
                            else
                            {
                                groups = groups
                                    .OrderByDescending(g => g.Name)
                                    .Skip((request.page - 1) * request.rows)
                                    .Take(request.rows)
                                    .ToList();
                            }
                            break;
                        }
                    case "Type":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                groups = groups.OrderBy(g => g.Type)
                                    .Skip((request.page - 1) * request.rows)
                                    .Take(request.rows)
                                    .ToList();
                            }
                            else
                            {
                                groups = groups
                                    .OrderByDescending(g => g.Type)
                                    .Skip((request.page - 1) * request.rows)
                                    .Take(request.rows)
                                    .ToList();
                            }
                            break;
                        }
                    case "LastAttended":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                groups = groups.OrderBy(g => g.LastAttended)
                                    .Skip((request.page - 1) * request.rows)
                                    .Take(request.rows)
                                    .ToList();
                            }
                            else
                            {
                                groups = groups
                                    .OrderByDescending(g => g.LastAttended)
                                    .Skip((request.page - 1) * request.rows)
                                    .Take(request.rows)
                                    .ToList();
                            }
                            break;
                        }
                    case "Leader":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                groups = groups.OrderBy(g => g.Leader)
                                    .Skip((request.page - 1) * request.rows)
                                    .Take(request.rows)
                                    .ToList();
                            }
                            else
                            {
                                groups = groups
                                    .OrderByDescending(g => g.Leader)
                                    .Skip((request.page - 1) * request.rows)
                                    .Take(request.rows)
                                    .ToList();
                            }
                            break;
                        }
                    case "Administrator":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                groups = groups.OrderBy(g => g.Administrator)
                                    .Skip((request.page - 1) * request.rows)
                                    .Take(request.rows)
                                    .ToList();
                            }
                            else
                            {
                                groups = groups
                                    .OrderByDescending(g => g.Administrator)
                                    .Skip((request.page - 1) * request.rows)
                                    .Take(request.rows)
                                    .ToList();
                            }
                            break;
                        }
                }

                JqGridData groupsGridData = new JqGridData()
                {
                    total = (int)Math.Ceiling((float)totalRecords / (float)request.rows),
                    page = request.page,
                    records = totalRecords,
                    rows = (from g in groups
                            select new JqGridRow()
                            {
                                id = g.GroupId.ToString(),
                                cell = new string[] {
                                                    g.GroupId.ToString(),
                                                    g.Name,
                                                    g.Type,
                                                    g.LastAttended,
                                                    g.Leader,
                                                    g.Administrator,
                                                    g.PrimaryGroup.ToString()
                                }
                            }).ToArray()
                };
                return groupsGridData;
            }
        }

        public static JqGridData FetchChurchListJQGrid(Person currentPerson, JqGridRequest request)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var rolesToInclude = context
                    .PermissionRoles
                    .Where(p => p.PermissionId == (int)Permissions.IncludeInChurchList && p.Role.ChurchId == currentPerson.ChurchId)
                    .Select(p=>p.RoleId)
                    .ToList();

                var people = (from p in context.People.Include("Family").Include("PersonOptionalFields")
                              from c in p.PersonChurches
                              where c.ChurchId == currentPerson.ChurchId
                              && rolesToInclude.Contains(c.RoleId)
                              select p);

                if (!(currentPerson.HasPermission(Permissions.ViewChurchContactDetails)))
                {
                    if(!currentPerson.HasPermission(Permissions.ViewGroupContactDetails))
                        throw new Exception("You do not have permission to view contact details");
                    //Get the groups
                    var groups = (from pg in context.PersonGroups
                                  where pg.PersonId == currentPerson.PersonId
                                  select pg.GroupId).ToList();

                    people = (from p in people
                              from pg in p.PersonGroups
                              where groups.Contains(pg.GroupId)
                              select p);
                }

                if (request._search)
                {
                    switch (request.searchField)
                    {
                        case "search":
                            {
                                people = Filters.ApplyNameSearch(request.searchString, people);
                                break;
                            }
                        case "homegroup":
                            {
                                var homegroupId = (from pg in context.PersonGroups
                                                   where pg.PersonId == currentPerson.PersonId
                                                   select pg.GroupId).FirstOrDefault();

                                if (homegroupId > 0)
                                {
                                    people = (from p in context.People.Include("Family").Include("PersonOptionalFields")
                                              from c in p.PersonChurches
                                              join pg in context.PersonGroups
                                              on p.PersonId equals pg.PersonId
                                              where c.ChurchId == currentPerson.ChurchId
                                              && pg.GroupId == homegroupId
                                              select p);
                                }
                                break;
                            }
                    }
                }

                int totalRecords = people.Count();

                switch (request.sidx)
                {
                    case "Firstname":
                        {
                            people = request.sord.ToLower() == "asc" ? people.OrderBy(p => p.Firstname).Skip((request.page - 1) * request.rows).Take(request.rows) : people.OrderByDescending(p => p.Firstname).Skip((request.page - 1) * request.rows).Take(request.rows);
                            break;
                        }
                    case "Surname":
                        {
                            people = request.sord.ToLower() == "asc" ? people.OrderBy(p => p.Family.FamilyName).Skip((request.page - 1) * request.rows).Take(request.rows) : people.OrderByDescending(p => p.Family.FamilyName).Skip((request.page - 1) * request.rows).Take(request.rows);
                            break;
                        }
                    case "Email":
                        {
                            people = request.sord.ToLower() == "asc" ? people.OrderBy(p => p.Email).Skip((request.page - 1) * request.rows).Take(request.rows) : people.OrderByDescending(p => p.Email).Skip((request.page - 1) * request.rows).Take(request.rows);
                            break;
                        }
                }

                var membersGridData = new JqGridData()
                {
                    total = (int)Math.Ceiling((float)totalRecords / request.rows),
                    page = request.page,
                    records = totalRecords,
                    rows = (from p in people.AsEnumerable()
                            select new JqGridRow()
                            {
                                id = p.PersonId.ToString(),
                                cell = new string[] {
                                                    p.PersonId.ToString(),
                                                    p.Firstname,
                                                    p.Family.FamilyName,
                                                    p.Family.HomePhone,
                                                    p.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int)OptionalFields.CellPhone)==null?"":p.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int)OptionalFields.CellPhone).Value,
                                                    p.Email
                                                }
                            }).ToArray()
                };
                return membersGridData;
            }
        }

        public static List<PersonListViewModel> FetchPeople(Person currentPerson, int roleId)
        {
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                return (from p in context.People.Include("Family").Include("PersonGroups")
                        from c in p.PersonChurches
                        where c.ChurchId == currentPerson.ChurchId
                          && (c.RoleId == roleId)
                        orderby p.Family.FamilyName, p.PersonId
                        select new PersonListViewModel
                        {
                            PersonId = p.PersonId,
                            FamilyId = p.FamilyId,
                            Firstname = p.Firstname,
                            Surname = p.Family.FamilyName,
                            HomePhone = p.Family.HomePhone,
                            CellPhone = p.PersonOptionalFields.FirstOrDefault(cp => cp.OptionalFieldId == (int)OptionalFields.CellPhone).Value,
                            WorkPhone = p.PersonOptionalFields.FirstOrDefault(cp => cp.OptionalFieldId == (int)OptionalFields.WorkPhone).Value,
                            Email = p.Email
                        }).ToList();
            }
        }

        public static JqGridData FetchPeopleJQGrid(Person currentPerson, JqGridRequest request, int roleId)
        {
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var people = (from p in context.People.Include("Family").Include("PersonGroups")
                              from pc in p.PersonChurches
                              where pc.ChurchId == currentPerson.ChurchId
                                && (pc.RoleId == roleId)
                              select p);

                if (request._search)
                {
                    foreach (var rule in request.filters.rules)
                    {
                        var ruleData = rule.data;
                        //If we use rule.data throughout we get some strange errors in the SQL that Linq generates
                        switch (rule.field)
                        {
                            case "Group":
                                {
                                    var groupIds = (from g in context.Groups
                                                    where g.Name.Contains(ruleData)
                                                    select g.GroupId).ToList();

                                    people = (from p in people
                                              from pg in p.PersonGroups
                                              where groupIds.Contains(pg.GroupId)
                                              select p);
                                    break;
                                }
                            case "Firstname":
                                {
                                    people = (from p in people
                                              where p.Firstname.Contains(ruleData)
                                              select p);
                                    break;
                                }
                            case "Surname":
                                {
                                    people = (from p in people
                                              where p.Family.FamilyName.Contains(ruleData)
                                              select p);
                                    break;
                                }
                            case "Date":
                                {
                                    DateTime dStart;
                                    if (DateTime.TryParse(ruleData, out dStart))
                                    {
                                        var dEnd = dStart.AddDays(1);
                                        people = (from p in people
                                                  where p.Created >= dStart && p.Created < dEnd
                                                  select p);
                                    }
                                    break;
                                }
                            case "Site":
                                {
                                    people = (from p in people
                                              where p.Site.Name.Contains(ruleData)
                                              select p);
                                    break;
                                }
                            case "HomePhone":
                                {
                                    people = (from p in people
                                              where p.Family.HomePhone.Contains(ruleData)
                                              select p);
                                    break;
                                }
                            case "CellPhone":
                                {
                                    people = (from p in people
                                              from po in p.PersonOptionalFields
                                              where (po.OptionalFieldId == (int)OptionalFields.CellPhone
                                                     && po.Value.Contains(ruleData))
                                              select p);
                                    break;
                                }
                            case "Email":
                                {
                                    people = (from p in people
                                              where p.Email.Contains(ruleData)
                                              select p);
                                    break;
                                }
                        }
                    }
                }

                int totalRecords = people.Count();

                switch (request.sidx)
                {
                    case "Firstname":
                        {
                            people = request.sord.ToLower() == "asc" ? people.OrderBy(p => p.Firstname).Skip((request.page - 1) * request.rows).Take(request.rows) : people.OrderByDescending(p => p.Firstname).Skip((request.page - 1) * request.rows).Take(request.rows);
                            break;
                        }
                    case "Surname":
                        {
                            people = request.sord.ToLower() == "asc" ? people.OrderBy(p => p.Family.FamilyName).Skip((request.page - 1) * request.rows).Take(request.rows) : people.OrderByDescending(p => p.Family.FamilyName).Skip((request.page - 1) * request.rows).Take(request.rows);
                            break;
                        }
                    case "Date":
                        {
                            people = request.sord.ToLower() == "asc" ? people.OrderBy(p => p.Created).Skip((request.page - 1) * request.rows).Take(request.rows) : people.OrderByDescending(p => p.Created).Skip((request.page - 1) * request.rows).Take(request.rows);
                            break;
                        }
                    case "Group":
                        {
                            people = request.sord.ToLower() == "asc" ? people.OrderBy(p => p.PersonGroups.FirstOrDefault().Group.Name).Skip((request.page - 1) * request.rows).Take(request.rows) : people.OrderByDescending(p => p.PersonGroups.FirstOrDefault().Group.Name).Skip((request.page - 1) * request.rows).Take(request.rows);
                            break;
                        }
                    case "Site":
                        {
                            people = request.sord.ToLower() == "asc" ? people.OrderBy(p => p.Site.Name).Skip((request.page - 1) * request.rows).Take(request.rows) : people.OrderByDescending(p => p.Site.Name).Skip((request.page - 1) * request.rows).Take(request.rows);
                            break;
                        }
                    case "HomePhone":
                        {
                            people = request.sord.ToLower() == "asc" ? people.OrderBy(p => p.Family.HomePhone).Skip((request.page - 1) * request.rows).Take(request.rows) : people.OrderByDescending(p => p.Family.HomePhone).Skip((request.page - 1) * request.rows).Take(request.rows);
                            break;
                        }
                    case "Email":
                        {
                            people = request.sord.ToLower() == "asc" ? people.OrderBy(p => p.Email).Skip((request.page - 1) * request.rows).Take(request.rows) : people.OrderByDescending(p => p.Email).Skip((request.page - 1) * request.rows).Take(request.rows);
                            break;
                        }
                }

                var peopleGridData = new JqGridData()
                    {
                        total = (int)Math.Ceiling((float)totalRecords / request.rows),
                        page = request.page,
                        records = totalRecords,
                        rows = (from p in people.AsEnumerable()
                                select new JqGridRow()
                                {
                                    id = p.PersonId.ToString(),
                                    cell = new string[] {
                                                        p.PersonId.ToString(),
                                                        p.Firstname,
                                                        p.Family.FamilyName,
                                                        p.Family.HomePhone,
                                                        p.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int)OptionalFields.CellPhone)==null?"":p.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int)OptionalFields.CellPhone).Value,
                                                        p.Email,
                                                        p.Created.ToString("dd MMM yyyy"),
                                                        p.PersonGroups.Count > 1 ? "Multiple" : (p.PersonGroups.Count==0 ? "None" : p.PersonGroups.First().Group.Name),
                                                        p.Site== null ? string.Empty : p.Site.Name
                                    }
                                }).ToArray()
                    };
                            
                return peopleGridData;
            }
        }

        public static List<string> FetchChurchEmailAddresses(Person currentPerson, bool search, string searchField, string searchString)
        {
            var validEmails = new List<string>();

            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var personList = FetchChurchList(currentPerson, search, searchField, searchString, context);

                var emailList = (from p in personList
                                 where p.Email != null
                                     && p.Email != string.Empty
                                 orderby p.Family.FamilyName, p.PersonId
                                 select p.Email)
                                .Distinct()
                                .ToList();

                foreach (var email in emailList.Where(email => Utils.ValidEmailAddress(email) && !validEmails.Contains(email)))
                {
                    validEmails.Add(email);
                }
            }

            return validEmails;
        }

        public static IEnumerable<string> FetchChurchCellPhoneNos(Person currentPerson, bool search, string searchField, string searchString)
        {
            var validatedNumbers = new List<string>();
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var personList = FetchChurchList(currentPerson, search, searchField, searchString, context);

                var cellPhoneList = (from p in personList
                                     join po in context.PersonOptionalFields
                                         on p.PersonId equals po.PersonId
                                     where po.OptionalFieldId == (int)OptionalFields.CellPhone
                                         && po.Value != null
                                     orderby p.Family.FamilyName, p.PersonId
                                     select po.Value)
                                    .Distinct()
                                    .ToList();
                
                foreach (var cellPhoneNo in cellPhoneList.Where(cellPhoneNo => cellPhoneNo != null && cellPhoneNo.Trim() != string.Empty).Where(cellPhoneNo => !validatedNumbers.Contains(cellPhoneNo)))
                {
                    validatedNumbers.Add(cellPhoneNo);
                }
            }

            return validatedNumbers;
        }

        public static List<PersonListViewModel> FetchChurchList(Person currentPerson, bool search, string searchField, string searchString)
        {
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var personList = FetchChurchList(currentPerson, search, searchField, searchString, context);

                return (from p in personList
                        orderby p.Family.FamilyName, p.PersonId
                        select new PersonListViewModel
                        {
                            PersonId = p.PersonId,
                            FamilyId = p.FamilyId,
                            Firstname = p.Firstname,
                            Surname = p.Family.FamilyName,
                            HomePhone = p.Family.HomePhone,
                            CellPhone = p.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int)OptionalFields.CellPhone).Value,
                            WorkPhone = p.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int)OptionalFields.WorkPhone).Value,
                            Email = p.Email
                        }).ToList();
            }
        }

        public static IEnumerable<PersonViewModel> FetchChurchListForTablet(Person currentPerson)
        {
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var personList = FetchChurchList(currentPerson, false, string.Empty, string.Empty, context);
                return (from p in personList
                        orderby p.Family.FamilyName, p.PersonId
                        select new PersonViewModel
                        {
                            PersonId          = p.PersonId,
                            FamilyId          = p.FamilyId,
                            Firstname         = p.Firstname,
                            Surname           = p.Family.FamilyName,
                            HomePhone         = p.Family.HomePhone,
                            CellPhone         = p.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int)OptionalFields.CellPhone).Value,
                            WorkPhone         = p.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int)OptionalFields.WorkPhone).Value,
                            Email             = p.Email,
                            Address1          = p.Family.Address.Line1,
                            Address2          = p.Family.Address.Line2,
                            Address3          = p.Family.Address.Line3,
                            Address4          = p.Family.Address.Line4,
                            Anniversary_Value = p.Anniversary,
                            DateOfBirth_Value = p.DateOfBirth,
                            Gender            = p.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int)OptionalFields.Gender).Value,
                            HeardAbout        = p.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int)OptionalFields.HeardAbout).Value,
                            FacebookId        = p.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int)OptionalFields.Facebook).Value,
                            Occupation        = p.Occupation,
                            Site              = p.Site.Name,
                            Skype             = p.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int)OptionalFields.Skype).Value,
                            Twitter           = p.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int)OptionalFields.Twitter).Value,
                            RoleName          = p.PersonChurches.FirstOrDefault(pc => pc.ChurchId == currentPerson.ChurchId).Role.DisplayName,
                            GroupName         = p.PersonGroups.Count(pg => pg.Group.ChurchId == currentPerson.ChurchId) > 1 ? "Multiple" : (p.PersonGroups.Count(pg => pg.Group.ChurchId == currentPerson.ChurchId) == 0 ? "None" : p.PersonGroups.FirstOrDefault(pg => pg.Group.ChurchId == currentPerson.ChurchId).Group.Name)
                        }).ToList();
            }
        }

        public static IEnumerable<PersonViewModel> FetchExtendedChurchList(Person currentPerson)
        {
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var personList = FetchExtendedChurchList(currentPerson, context);
                if (personList == null)
                {
                    return new List<PersonViewModel>();
                }

                return (from p in personList
                        orderby p.Family.FamilyName, p.PersonId
                        select new PersonViewModel
                        {
                            PersonId          = p.PersonId,
                            FamilyId          = p.FamilyId,
                            Firstname         = p.Firstname,
                            Surname           = p.Family.FamilyName,
                            HomePhone         = p.Family.HomePhone,
                            CellPhone         = p.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int)OptionalFields.CellPhone).Value,
                            WorkPhone         = p.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int)OptionalFields.WorkPhone).Value,
                            Email             = p.Email,
                            Address1          = p.Family.Address.Line1.Replace(",", string.Empty),
                            Address2          = p.Family.Address.Line2.Replace(",", string.Empty),
                            Address3          = p.Family.Address.Line3.Replace(",", string.Empty),
                            Address4          = p.Family.Address.Line4.Replace(",", string.Empty),
                            Anniversary_Value = p.Anniversary,
                            DateOfBirth_Value = p.DateOfBirth,
                            Gender            = p.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int)OptionalFields.Gender).Value,
                            HeardAbout        = p.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int)OptionalFields.HeardAbout).Value,
                            FacebookId        = p.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int)OptionalFields.Facebook).Value,
                            Occupation        = p.Occupation,
                            Site              = p.Site.Name,
                            Skype             = p.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int)OptionalFields.Skype).Value,
                            Twitter           = p.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int)OptionalFields.Twitter).Value,
                            RoleName          = p.PersonChurches.FirstOrDefault(pc=>pc.ChurchId==currentPerson.ChurchId).Role.Name,
                            GroupName         = p.PersonGroups.Count(pg => pg.Group.ChurchId == currentPerson.ChurchId) == 0 ? 
                                                "None" : 
                                                p.PersonGroups.FirstOrDefault(pg => pg.Group.ChurchId == currentPerson.ChurchId && pg.PrimaryGroup).Group == null ? 
                                                p.PersonGroups.FirstOrDefault(pg => pg.Group.ChurchId == currentPerson.ChurchId).Group.Name : 
                                                p.PersonGroups.FirstOrDefault(pg => pg.Group.ChurchId == currentPerson.ChurchId && pg.PrimaryGroup).Group.Name
                        }).ToList();
            }
        }


        private static IQueryable<Person> FetchExtendedChurchList(Person currentPerson, oikonomosEntities context)
        {
            if (!currentPerson.HasPermission(Permissions.ViewChurchContactDetails))
            {
                return null;
            }

            return (from p in context.People
                    from c in p.PersonChurches
                    where c.ChurchId == currentPerson.ChurchId
                    select p);
        }
        
        private static IQueryable<Person> FetchChurchList(Person currentPerson, bool search, string searchField, string searchString, oikonomosEntities context)
        {
            var showWholeChurchList = (currentPerson.HasPermission(Permissions.ViewChurchContactDetails));
            if (!showWholeChurchList)
            {
                showWholeChurchList = (from c in context.ChurchOptionalFields
                                       where c.ChurchId == currentPerson.ChurchId
                                       && c.OptionalFieldId == (int)OptionalFields.ShowWholeChurch
                                       select c.Visible).FirstOrDefault();
            }

            var personList = (from p in context.People
                              from c in p.PersonChurches
                              join permissions in context.PermissionRoles
                                  on c.RoleId equals permissions.RoleId
                              where c.ChurchId == currentPerson.ChurchId
                                  && permissions.PermissionId == (int)Permissions.IncludeInChurchList
                              select p);

            if (!showWholeChurchList || (search && searchField == "homegroup"))
            {
                //Get the groups
                var groups = (from pg in context.PersonGroups
                              where pg.PersonId == currentPerson.PersonId
                              select pg.GroupId).ToList();

                personList = (from p in personList
                              from pg in p.PersonGroups
                              where groups.Contains(pg.GroupId)
                              select p);
            }
            else
            {
                if (search && searchField == "search")
                {
                    personList = Filters.ApplyNameSearch(searchString, personList);
                }
            }
            return personList;
        }

        public static void SavePersonFacebookDetails(Person person, long facebookId, DateTime? birthday)
        {
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var facebookOptionalField = new PersonOptionalField();
                facebookOptionalField.PersonId = person.PersonId;
                facebookOptionalField.OptionalFieldId = (int)OptionalFields.Facebook;
                facebookOptionalField.Value = facebookId.ToString();
                facebookOptionalField.Created = DateTime.Now;
                facebookOptionalField.Changed = DateTime.Now;

                context.PersonOptionalFields.AddObject(facebookOptionalField);

                if (birthday != null)
                {
                    var personToUpdate = (from p in context.People
                                  where p.PersonId == person.PersonId
                                  select p).FirstOrDefault();

                    if (personToUpdate.DateOfBirth == null || personToUpdate.DateOfBirth.Value.Year == 1900)
                    {
                        personToUpdate.DateOfBirth = birthday.Value;
                    }
                }

                context.SaveChanges();
            }
        }

        public static void SavePersonFacebookId(int personId, string facebookId)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var facebookOptionalField = new PersonOptionalField();
                facebookOptionalField.PersonId = personId;
                facebookOptionalField.OptionalFieldId = (int)OptionalFields.Facebook;
                facebookOptionalField.Value = facebookId;
                facebookOptionalField.Created = DateTime.Now;
                facebookOptionalField.Changed = DateTime.Now;

                context.PersonOptionalFields.AddObject(facebookOptionalField);
                context.SaveChanges();
            }
        }

        public static AutoCompleteViewModel[] FetchFamilyAutoComplete(string term, int churchId)
        {
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {

                return (from f in context.Families
                        join p in context.People
                            on f.FamilyId equals p.FamilyId
                        join pc in context.PersonChurches
                            on p.PersonId equals pc.PersonId
                        where pc.ChurchId == churchId
                              && f.FamilyName.Contains(term)
                        orderby f.FamilyName
                        select new AutoCompleteViewModel
                                   {
                                       id = f.FamilyId,
                                       label = f.FamilyName,
                                       value = f.FamilyName
                                   }).Take(12).ToArray();

            }
        }

        public static AutoCompleteViewModel[] FetchPersonAutoComplete(string term, Person currentPerson, bool wholeChurch)
        {
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var query = (from p in context.People.Include("PersonOptionalField").Include("Address")
                             from c in p.PersonChurches
                             join r in context.Roles
                                on c.RoleId equals r.RoleId
                             where c.ChurchId == currentPerson.ChurchId
                               && r.ChurchId == currentPerson.ChurchId
                             select p);

                if (term.Contains(" "))
                {
                    var searchStrings = term.Split(' ');
                    var searchString1 = searchStrings[0];
                    var searchString2 = searchStrings[1];
                    query             = query.Where(p => p.Firstname.Contains(searchString1) && p.Family.FamilyName.Contains(searchString2));
                }
                else
                {
                    query = query.Where(p => p.Firstname.Contains(term) || p.Family.FamilyName.Contains(term));
                }

                if (!wholeChurch)
                {
                    //Find out the persons role
                    if (currentPerson.HasPermission(Permissions.EditChurchPersonalDetails))
                    {
                        //No filter required
                        query = query.Take(12);
                    }
                    else if (currentPerson.HasPermission(Permissions.EditGroupPersonalDetails))
                    {
                        var grp = (from g in context.Groups
                                       where g.LeaderId == currentPerson.PersonId
                                       || g.AdministratorId == currentPerson.PersonId
                                       select g).FirstOrDefault();

                        if (grp == null)
                        {
                            return new AutoCompleteViewModel[0];
                        }
                        //Filter for the group
                        query = (from q in query
                                 join pg in context.PersonGroups
                                    on q.PersonId equals pg.PersonId
                                 where pg.GroupId == grp.GroupId
                                 select q).Take(12);
                    }
                    else
                    {
                        return new AutoCompleteViewModel[0];
                    }
                }

                return (from p in query.OrderBy(p => p.Firstname)
                        select new AutoCompleteViewModel
                        {
                            id = p.PersonId,
                            label = p.Firstname + " " + p.Family.FamilyName,
                            value = p.Firstname + " " + p.Family.FamilyName
                        }).ToArray();
            }
        }

        public static void DeletePerson(int personId, Person currentPerson)
        {
            if(!currentPerson.HasPermission(Permissions.DeletePerson))
                return;
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                if (RemovePersonFromChurchSpecificTables(personId, currentPerson, context))
                {
                    DeletePerson(personId, context, currentPerson);
                }
                context.SaveChanges();
            }
        }

        private static void DeletePerson(int personId, oikonomosEntities context, Person currentPerson)
        {
            DeleteAddressIfNotBeingUsed(personId, context);
            DeleteCommentsAboutPerson(personId, context);
            UpdateCommentsMadeByThisPerson(personId, context, currentPerson);
            DeleteRelationships(personId, context);
            DeleteOptionalFields(personId, context);
            DeleteMessages(personId, context);

            var person = (from p in context.People
                          where p.PersonId == personId
                          select p).FirstOrDefault();

            var familyMembers = (from p in context.People
                                 where p.PersonId != personId
                                 && p.FamilyId == person.FamilyId
                                 select p).ToList();

            context.DeleteObject(person);

            if (familyMembers.Count != 0) return;
            var familyToDelete = (from f in context.Families
                                  join p in context.People
                                      on f.FamilyId equals p.FamilyId
                                  where p.PersonId == personId
                                  select f).FirstOrDefault();

            context.DeleteObject(familyToDelete);
        }

        private static void DeleteMessages(int personId, oikonomosEntities context)
        {
            var messageRecepients = context.MessageRecepients.Where(m => m.MessageTo == personId);
            foreach (var messageRecepient in messageRecepients)
            {
                var message = context.Messages.FirstOrDefault(m=>m.MessageId == messageRecepient.MessageId);
                context.DeleteObject(messageRecepient);
                if (message == null) continue;
                var remainingMessageRecepients = message.MessageRecepients.Count();
                if (remainingMessageRecepients == 0)
                    context.DeleteObject(message);
            }
        }

        private static void DeleteOptionalFields(int personId, oikonomosEntities context)
        {
            var optionalFields = (from pc in context.PersonOptionalFields
                                  where pc.PersonId == personId
                                  select pc);

            foreach (PersonOptionalField optionalField in optionalFields)
            {
                context.DeleteObject(optionalField);
            }
        }

        private static void DeleteRelationships(int personId, oikonomosEntities context)
        {
//Remove all the relationships
            var relationships = (from pr in context.PersonRelationships
                                 where pr.PersonId == personId
                                 select pr);

            foreach (PersonRelationship relationship in relationships)
            {
                context.DeleteObject(relationship);
            }

            var relatedTo = (from pr in context.PersonRelationships
                             where pr.PersonRelatedToId == personId
                             select pr);

            foreach (PersonRelationship rel in relatedTo)
            {
                context.DeleteObject(rel);
            }
        }

        private static void DeleteAddressIfNotBeingUsed(int personId, oikonomosEntities context)
        {
//Check to see if this address is being used
            var address = (from p in context.People
                           join f in context.Families
                               on p.FamilyId equals f.FamilyId
                           join a in context.Addresses
                               on f.AddressId equals a.AddressId
                           where p.PersonId != personId
                           select p).ToList();

            if (address.Count == 0)
            {
                var addressToDelete = (from a in context.Addresses
                                       join f in context.Families
                                           on a.AddressId equals f.AddressId
                                       join p in context.People
                                           on f.FamilyId equals p.FamilyId
                                       where p.PersonId == personId
                                       select a).FirstOrDefault();

                context.DeleteObject(addressToDelete);
            }
        }

        private static void UpdateCommentsMadeByThisPerson(int personId, oikonomosEntities context, Person currentPerson)
        {
//Update comments made by this person
            var commentsMadeByThisPerson = (from c in context.Comments
                                            where c.MadeByPersonId == personId
                                            select c);

            foreach (var comment in commentsMadeByThisPerson)
            {
                comment.MadeByPersonId = currentPerson.PersonId;
            }
        }

        private static void DeleteCommentsAboutPerson(int personId, oikonomosEntities context)
        {
            var comments = context.Comments.Where(c => c.AboutPersonId == personId);
            foreach (var comment in comments)
            {
                context.DeleteObject(comment);
            }
        }

        private static bool RemovePersonFromChurchSpecificTables(int personId, Person currentPerson, oikonomosEntities context)
        {
            const int tableId = (int)Tables.Person;

            //If this person has created or changed any events - change to the person deleting them
            foreach (var createdEvent in context.OldEvents.Where(e => e.CreatedByPersonId == personId))
                createdEvent.CreatedByPersonId = currentPerson.PersonId;

            foreach (var changedEvent in context.OldEvents.Where(e => e.ChangedByPersonId == personId))
                changedEvent.ChangedByPersonId = currentPerson.PersonId;

            var events = (from e in context.OldEvents
                          where e.Reference == personId
                          && e.TableId == tableId
                          && e.ChurchId == currentPerson.ChurchId
                          select e);

            foreach (var eventToDelete in events)
            {
                context.DeleteObject(eventToDelete);
            }

            //Remove them from groups
            var groups = (from g in context.Groups
                          from pg in g.PersonGroups
                          where pg.PersonId == personId
                          && g.ChurchId == currentPerson.ChurchId
                          select pg);

            foreach (PersonGroup personGroup in groups)
            {
                context.DeleteObject(personGroup);
            }

            //Remove them from roles
            var personRoles = (from r in context.Roles
                               from pr in r.PersonChurches
                               where pr.PersonId == personId
                               && r.ChurchId == currentPerson.ChurchId
                               select pr);

            foreach (var personRole in personRoles)
            {
                context.DeleteObject(personRole);
            }

            //Remove them from any leadership or admin roles in Home Groups
            var groupsThePersonLeadsOrAdministrates = (from g in context.Groups
                                                       where (g.AdministratorId == personId ||
                                                             g.LeaderId == personId)
                                                             && g.ChurchId == currentPerson.ChurchId
                                                       select g);

            foreach (var group in groupsThePersonLeadsOrAdministrates)
            {
                if (group.LeaderId == personId)
                    group.LeaderId = null;
                if (group.AdministratorId == personId)
                    group.AdministratorId = null;
            }

            var person = context.People.First(p => p.PersonId == personId);
            return person.PersonChurches.Count == 0;
        }
    }
}