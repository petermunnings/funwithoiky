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
using System.Threading.Tasks;
using System.Net.Mail;
using oikonomos.data.Services;
using Lib.Web.Mvc.JQuery.JqGrid;

namespace oikonomos.data.DataAccessors
{
    public static class PersonDataAccessor
    {
        public static JqGridData FetchGroupsForPersonJQGrid(Person currentPerson, int personId, JqGridRequest request)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
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
                                                     Leader = g.Leader.Firstname + " " + g.Leader.Family.FamilyName
                                                 })
                                                 .ToList();

                foreach (PersonGroupModel pg in groups)
                {
                    string groupIdAsString = pg.GroupId.ToString();
                    DateTime lastDateAttended = (from e in context.Events
                                            where e.Reference == personId
                                              && e.Value == groupIdAsString
                                            orderby e.EventDate descending
                                            select e.EventDate)
                                            .FirstOrDefault();

                    pg.LastAttended = lastDateAttended == DateTime.MinValue ? "Never" : lastDateAttended.ToString("dd MMM yyyy");
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
                                                    g.Administrator
                                }
                            }).ToArray()
                };
                return groupsGridData;
            }
        }
 

        
        public static string ResetPassword(string emailAddress)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var person = (from p in context.People.Include("Church")
                              where p.Email == emailAddress
                              select p).ToList();
                
                if (person.Count == 1)
                {
                    string password = RandomPasswordGenerator.Generate(RandomPasswordOptions.AlphaNumeric);
                    person[0].PasswordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");
                    context.SaveChanges();

                    return Email.SendResetPasswordEmail(person[0], password);
                }
                else
                {
                    return "Your email address could not be found in the database.  Please contact your church administrator for further assistance";
                }
            }
        }
        
        public static bool SendEmailAndPassword(Person currentPerson, int personId, ref string message)
        {
            if (personId == 0)
            {
                message = "You need to save the person before sending the email";
                return false;
            }

            if (!currentPerson.HasPermission(Permissions.SendEmailAndPassword))
            {
                message = "You don't have permission to perform this action";
                return false;
            }

            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var church = (from c in context.Churches
                              where c.ChurchId == currentPerson.ChurchId
                              select c).FirstOrDefault();
                if (church == null)
                {
                    message = "Error sending Email";
                    return false;
                }

                var personToUpdate = (from p in context.People
                                      where p.PersonId == personId
                                      select p).FirstOrDefault();

                if (personToUpdate == null)
                {
                    message = "Error sending Email";
                    return false;
                }

                if (personToUpdate.HasPermission(Permissions.Login))
                {
                    if (personToUpdate.HasValidEmail())
                    {

                        SendEmailAndPassword(personToUpdate.Firstname,
                            personToUpdate.Family.FamilyName,
                            church,
                            personToUpdate.Email,
                            personToUpdate);

                        context.SaveChanges();
                        message = "Email sent succesfully";
                        return true;
                    }
                    else
                    {
                        message = "Invalid Email address";
                        return false;
                    }
                }
                else
                {
                    message = string.Format("You cannot send login details to a person with a role of {0}", personToUpdate.PersonRoles.FirstOrDefault().Role.Name);
                    return false;
                }
            }
        }

        public static JqGridData FetchChurchListJQGrid(Person currentPerson, JqGridRequest request)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var people = (from p in context.People.Include("Family").Include("PersonOptionalFields")
                              where p.ChurchId == currentPerson.ChurchId
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
                                              join pg in context.PersonGroups
                                              on p.PersonId equals pg.PersonId
                                              where p.ChurchId == currentPerson.ChurchId
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
                            if (request.sord.ToLower() == "asc")
                            {
                                people = people.OrderBy(p => p.Firstname).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                people = people.OrderByDescending(p => p.Firstname).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "Surname":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                people = people.OrderBy(p => p.Family.FamilyName).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                people = people.OrderByDescending(p => p.Family.FamilyName).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "Email":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                people = people.OrderBy(p => p.Email).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                people = people.OrderByDescending(p => p.Email).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                }

                JqGridData membersGridData = new JqGridData()
                {
                    total = (int)Math.Ceiling((float)totalRecords / (float)request.rows),
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
                                                    p.PersonOptionalFields.Where<PersonOptionalField>(c => c.OptionalFieldId == (int)OptionalFields.CellPhone).FirstOrDefault()==null?"":p.PersonOptionalFields.Where<PersonOptionalField>(c => c.OptionalFieldId == (int)OptionalFields.CellPhone).FirstOrDefault().Value,
                                                    p.Email
                                                }
                            }).ToArray()
                };
                return membersGridData;
            }
        }

        public static List<PersonListViewModel> FetchPeople(Person currentPerson, int roleId)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                return (from p in context.People.Include("Family").Include("PersonGroups")
                        from pr in p.PersonRoles
                        where p.ChurchId == currentPerson.ChurchId
                          && (pr.RoleId == roleId)
                        orderby p.Family.FamilyName, p.PersonId
                        select new PersonListViewModel
                        {
                            PersonId = p.PersonId,
                            FamilyId = p.FamilyId,
                            Firstname = p.Firstname,
                            Surname = p.Family.FamilyName,
                            HomePhone = p.Family.HomePhone,
                            CellPhone = p.PersonOptionalFields.Where<PersonOptionalField>(c => c.OptionalFieldId == (int)OptionalFields.CellPhone).FirstOrDefault().Value,
                            WorkPhone = p.PersonOptionalFields.Where<PersonOptionalField>(c => c.OptionalFieldId == (int)OptionalFields.WorkPhone).FirstOrDefault().Value,
                            Email = p.Email
                        }).ToList();
            }
        }

        public static JqGridData FetchPeopleInGroupJQGrid(Person currentPerson, JqGridRequest request, int groupId)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var people = (from p in context.People.Include("Family").Include("PersonGroups")
                              from pg in p.PersonGroups
                              where p.ChurchId == currentPerson.ChurchId
                                && (pg.GroupId == groupId)
                              select p);

                if (request._search)
                {
                    foreach (JqGridFilterRule rule in request.filters.rules)
                    {
                        string ruleData = rule.data;
                        //If we use rule.data throughout we get some strange errors in the SQL that Linq generates
                        switch (rule.field)
                        {
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
                        }
                    }
                }

                int totalRecords = people.Count();

                switch (request.sidx)
                {
                    case "Firstname":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                people = people.OrderBy(p => p.Firstname).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                people = people.OrderByDescending(p => p.Firstname).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "Surname":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                people = people.OrderBy(p => p.Family.FamilyName).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                people = people.OrderByDescending(p => p.Family.FamilyName).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                }

                JqGridData peopleGridData = new JqGridData()
                {
                    total = (int)Math.Ceiling((float)totalRecords / (float)request.rows),
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
                                                        p.PersonOptionalFields.Where<PersonOptionalField>(c => c.OptionalFieldId == (int)OptionalFields.CellPhone).FirstOrDefault()==null?"":p.PersonOptionalFields.Where<PersonOptionalField>(c => c.OptionalFieldId == (int)OptionalFields.CellPhone).FirstOrDefault().Value,
                                                        p.Email
                                    }
                            }).ToArray()
                };

                return peopleGridData;
            }
        }


        public static JqGridData FetchPeopleJQGrid(Person currentPerson, JqGridRequest request, int roleId)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var people = (from p in context.People.Include("Family").Include("PersonGroups")
                              from pr in p.PersonRoles
                              where p.ChurchId == currentPerson.ChurchId
                                && (pr.RoleId == roleId)
                              select p);

                if (request._search)
                {
                    foreach (JqGridFilterRule rule in request.filters.rules)
                    {
                        string ruleData = rule.data;
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
                                        DateTime dEnd = dStart.AddDays(1);
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
                            if (request.sord.ToLower() == "asc")
                            {
                                people = people.OrderBy(p => p.Firstname).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                people = people.OrderByDescending(p => p.Firstname).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "Surname":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                people = people.OrderBy(p => p.Family.FamilyName).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                people = people.OrderByDescending(p => p.Family.FamilyName).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "Date":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                people = people.OrderBy(p => p.Created).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                people = people.OrderByDescending(p => p.Created).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "Group":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                people = people.OrderBy(p => p.PersonGroups.FirstOrDefault().Group.Name).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                people = people.OrderByDescending(p => p.PersonGroups.FirstOrDefault().Group.Name).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "Site":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                people = people.OrderBy(p => p.Site.Name).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                people = people.OrderByDescending(p => p.Site.Name).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "HomePhone":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                people = people.OrderBy(p => p.Family.HomePhone).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                people = people.OrderByDescending(p => p.Family.HomePhone).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "CellPhone":
                        {
                            //if (request.sord.ToLower() == "asc")
                            //{
                            //    people = people.OrderBy(p => p.Site.Name).Skip((request.page - 1) * request.rows).Take(request.rows);
                            //}
                            //else
                            //{
                            //    people = people.OrderByDescending(p => p.Site.Name).Skip((request.page - 1) * request.rows).Take(request.rows);
                            //}
                            break;
                        }
                    case "Email":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                people = people.OrderBy(p => p.Email).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                people = people.OrderByDescending(p => p.Email).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                }

                JqGridData peopleGridData = new JqGridData()
                    {
                        total = (int)Math.Ceiling((float)totalRecords / (float)request.rows),
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
                                                        p.PersonOptionalFields.Where<PersonOptionalField>(c => c.OptionalFieldId == (int)OptionalFields.CellPhone).FirstOrDefault()==null?"":p.PersonOptionalFields.Where<PersonOptionalField>(c => c.OptionalFieldId == (int)OptionalFields.CellPhone).FirstOrDefault().Value,
                                                        p.Email,
                                                        p.Created.ToString("dd MMM yyyy"),
                                                        p.PersonGroups.Count > 1 ? "Multiple" : (p.PersonGroups.Count==0 ? "None" : p.PersonGroups.FirstOrDefault().Group.Name),
                                                        p.Site== null ? string.Empty : p.Site.Name
                                    }
                                }).ToArray()
                    };
                            
                return peopleGridData;
            }
        }

        public static List<string> FetchChurchEmailAddresses(Person currentPerson, bool search, string searchField, string searchString)
        {
            List<string> validEmails = new List<string>();

            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var personList = FetchChurchList(currentPerson, search, searchField, searchString, context);

                var emailList = (from p in personList
                                 where p.Email != null
                                     && p.Email != string.Empty
                                 orderby p.Family.FamilyName, p.PersonId
                                 select p.Email)
                                .Distinct()
                                .ToList();

                foreach (string email in emailList)
                {
                    if (Utils.ValidEmailAddress(email) && !validEmails.Contains(email))
                    {
                        validEmails.Add(email);
                    }
                }
            }

            return validEmails;
        }

        public static List<string> FetchChurchCellPhoneNos(Person currentPerson, bool search, string searchField, string searchString)
        {
            List<string> validatedNumbers = new List<string>();
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
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
                
                foreach (string cellPhoneNo in cellPhoneList)
                {
                    string intNo = Utils.ConvertCellPhoneToInternational(cellPhoneNo, currentPerson.Church.Country);
                    if (!validatedNumbers.Contains(intNo))
                    {
                        validatedNumbers.Add(intNo);
                    }
                }
            }

            return validatedNumbers;
        }

        public static List<PersonListViewModel> FetchChurchList(Person currentPerson, bool search, string searchField, string searchString)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
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
                            CellPhone = p.PersonOptionalFields.Where<PersonOptionalField>(c => c.OptionalFieldId == (int)OptionalFields.CellPhone).FirstOrDefault().Value,
                            WorkPhone = p.PersonOptionalFields.Where<PersonOptionalField>(c => c.OptionalFieldId == (int)OptionalFields.WorkPhone).FirstOrDefault().Value,
                            Email = p.Email
                        }).ToList();
            }
        }

        public static List<PersonViewModel> FetchExtendedChurchList(Person currentPerson)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
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
                            PersonId = p.PersonId,
                            FamilyId = p.FamilyId,
                            Firstname = p.Firstname,
                            Surname = p.Family.FamilyName,
                            HomePhone = p.Family.HomePhone,
                            CellPhone = p.PersonOptionalFields.Where<PersonOptionalField>(c => c.OptionalFieldId == (int)OptionalFields.CellPhone).FirstOrDefault().Value,
                            WorkPhone = p.PersonOptionalFields.Where<PersonOptionalField>(c => c.OptionalFieldId == (int)OptionalFields.WorkPhone).FirstOrDefault().Value,
                            Email = p.Email,
                            Address1 = p.Family.Address.Line1,
                            Address2 = p.Family.Address.Line2,
                            Address3 = p.Family.Address.Line3,
                            Address4 = p.Family.Address.Line4,
                            Anniversary_Value = p.Anniversary,
                            DateOfBirth_Value = p.DateOfBirth,
                            Gender = p.PersonOptionalFields.Where<PersonOptionalField>(c => c.OptionalFieldId == (int)OptionalFields.Gender).FirstOrDefault().Value,
                            HeardAbout = p.PersonOptionalFields.Where<PersonOptionalField>(c => c.OptionalFieldId == (int)OptionalFields.HeardAbout).FirstOrDefault().Value,
                            Occupation = p.Occupation,
                            Site = p.Site.Name,
                            Skype = p.PersonOptionalFields.Where<PersonOptionalField>(c => c.OptionalFieldId == (int)OptionalFields.Skype).FirstOrDefault().Value,
                            Twitter = p.PersonOptionalFields.Where<PersonOptionalField>(c => c.OptionalFieldId == (int)OptionalFields.Twitter).FirstOrDefault().Value,
                            RoleName = (from pr in p.PersonRoles join r in context.Roles on pr.RoleId equals r.RoleId select r.Name).FirstOrDefault()
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
                    join pr in context.PersonRoles
                        on p.PersonId equals pr.PersonId
                    where p.ChurchId == currentPerson.ChurchId
                    select p);
        }
        
        private static IQueryable<Person> FetchChurchList(Person currentPerson, bool search, string searchField, string searchString, oikonomosEntities context)
        {
            bool showWholeChurchList = (currentPerson.HasPermission(Permissions.ViewChurchContactDetails));
            if (!showWholeChurchList)
            {
                showWholeChurchList = (from c in context.ChurchOptionalFields
                                       where c.ChurchId == currentPerson.ChurchId
                                       && c.OptionalFieldId == (int)OptionalFields.ShowWholeChurch
                                       select c.Visible).FirstOrDefault();
            }

            var personList = (from p in context.People
                              join pr in context.PersonRoles
                                  on p.PersonId equals pr.PersonId
                              join permissions in context.PermissionRoles
                                  on pr.RoleId equals permissions.RoleId
                              where p.ChurchId == currentPerson.ChurchId
                                  && permissions.PermissionId == (int)Permissions.Login
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
        
        public static Person Login(string email, string password, ref string message)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                Person loggedOnPerson = CheckEmailPassword(email, password, context);
                if (loggedOnPerson == null)
                {
                    message= "Invalid Email or Password";
                    return null;
                }
                else
                {
                    string fullName = loggedOnPerson.Firstname + " " + loggedOnPerson.Family.FamilyName;
                    message = "Welcome " + fullName + " from " + loggedOnPerson.Church.Name;
                    return loggedOnPerson;
                }
            }
        }

        public static string ChangePassword(int personId, string currentPassword, string newPassword)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                Person loggedOnPerson = CheckEmailPassword(personId, currentPassword, context);
                if (loggedOnPerson == null)
                {
                    return "Invalid Password";
                }
                else
                {
                    //Change password
                    string passwordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(newPassword, "sha1");
                    loggedOnPerson.PasswordHash = passwordHash;
                    loggedOnPerson.Changed = DateTime.Now;
                    context.SaveChanges();
                    return "Password succesfully changed";
                }
            }
        }

        public static List<FamilyMemberViewModel> FetchFamilyMembers(int personId, int familyId)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                return FetchFamilyMembers(personId, familyId, context);
            }
        }

        public static Person FetchPersonFromPublicId(string publicId)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var person = (from p in context.People.Include("Church").Include("PersonRoles")
                              where p.PublicId == publicId
                              select p).FirstOrDefault();

                if (person != null)
                {
                    bool isSysAdmin = person.HasPermission(Permissions.SystemAdministrator); //Work around because Eager loading is not working???
                    string church = person.Church.Name;
                    string surname = person.Family.FamilyName;
                }
                return person;
            }
        }

        public static Person FetchPersonFromFacebookId(long facebookId)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                string facebookValue = facebookId.ToString();
                var person = (from p in context.People
                              join po in context.PersonOptionalFields
                                on p.PersonId equals po.PersonId
                              where po.OptionalFieldId == (int)OptionalFields.Facebook
                                && po.Value == facebookValue
                              select p).FirstOrDefault();

                if (person != null)
                {
                    bool isSysAdmin = person.HasPermission(Permissions.SystemAdministrator); //Work around because Eager loading is not working???
                    string church = person.Church.Name;
                    string surname = person.Family.FamilyName;
                }
                return person;
            }
        }

        public static void SavePersonFacebookDetails(Person person, long facebookId, DateTime? birthday)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                PersonOptionalField facebookOptionalField = new PersonOptionalField();
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
                PersonOptionalField facebookOptionalField = new PersonOptionalField();
                facebookOptionalField.PersonId = personId;
                facebookOptionalField.OptionalFieldId = (int)OptionalFields.Facebook;
                facebookOptionalField.Value = facebookId;
                facebookOptionalField.Created = DateTime.Now;
                facebookOptionalField.Changed = DateTime.Now;

                context.PersonOptionalFields.AddObject(facebookOptionalField);
                context.SaveChanges();
            }
        }

        public static List<Person> FetchPersonFromName(string fullname, string firstname, string surname, string email)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var persons = (from p in context.People.Include("Church").Include("Family")
                               join f in context.Families
                                 on p.FamilyId equals f.FamilyId
                               where p.Firstname == firstname
                                 && f.FamilyName == surname
                                 && p.Email == email
                               select p).ToList();

                if (persons.Count == 0)
                {
                    string[] fullnames = fullname.Split(' ');
                    if (fullnames.Length > 2)
                    {
                        //Try and search for the person using the first two names in the full name
                        //Takes out the maiden surname
                        firstname = fullnames[0];
                        surname = fullnames[1];
                        persons = (from p in context.People.Include("Church").Include("Family")
                                   join f in context.Families
                                     on p.FamilyId equals f.FamilyId
                                   where p.Firstname == firstname
                                     && f.FamilyName == surname
                                     && p.Email == email
                                   select p).ToList();

                        if (persons.Count == 0)
                        {
                            firstname = fullnames[0];
                            surname = fullnames[fullnames.Length-1];
                            persons = (from p in context.People.Include("Church").Include("Family")
                                       join f in context.Families
                                         on p.FamilyId equals f.FamilyId
                                       where p.Firstname == firstname
                                         && f.FamilyName == surname
                                         && p.Email == email
                                       select p).ToList();
                        }
                    }
                }

                foreach (Person person in persons)
                {
                    bool isSysAdmin = person.HasPermission(Permissions.SystemAdministrator); //Work around because Eager loading is not working???
                    string church = person.Church.Name;
                    church = person.Family.FamilyName;
                }


                return persons;
            }
        }

        public static PersonViewModel FetchPersonViewModel(int personId, Person currentPerson)
        {
            //TODO - add security here.  Anyone could troll through the church database - even a visitor
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var churchId = (from p in context.People
                                where p.PersonId == personId
                                select p.ChurchId).FirstOrDefault();
                
                if (churchId != currentPerson.ChurchId)
                {
                    return null;
                }

                var familyId = (from p in context.People
                               where p.PersonId == personId
                               select p.FamilyId).FirstOrDefault();

                var person= (from p in context.People.Include("PersonOptionalField").Include("Family")
                        where p.PersonId == personId
                        select new PersonViewModel
                        {
                            PersonId = p.PersonId,
                            FamilyId = p.FamilyId,
                            Firstname = p.Firstname,
                            Surname = p.Family.FamilyName,
                            Email = p.Email,
                            DateOfBirth_Value = p.DateOfBirth,
                            Anniversary_Value = p.Anniversary,
                            HomePhone = p.Family.HomePhone,
                            CellPhone = p.PersonOptionalFields.Where<PersonOptionalField>(c => c.OptionalFieldId == (int)OptionalFields.CellPhone).FirstOrDefault().Value,
                            WorkPhone = p.PersonOptionalFields.Where<PersonOptionalField>(c => c.OptionalFieldId == (int)OptionalFields.WorkPhone).FirstOrDefault().Value,
                            Skype = p.PersonOptionalFields.Where<PersonOptionalField>(c => c.OptionalFieldId == (int)OptionalFields.Skype).FirstOrDefault().Value,
                            Twitter = p.PersonOptionalFields.Where<PersonOptionalField>(c => c.OptionalFieldId == (int)OptionalFields.Twitter).FirstOrDefault().Value,
                            FacebookId = p.PersonOptionalFields.Where<PersonOptionalField>(c => c.OptionalFieldId == (int)OptionalFields.Facebook).FirstOrDefault().Value,
                            Occupation = p.Occupation,
                            Gender = p.PersonOptionalFields.Where<PersonOptionalField>(c => c.OptionalFieldId == (int)OptionalFields.Gender).FirstOrDefault().Value,
                            Address1 = p.Family.Address.Line1,
                            Address2 = p.Family.Address.Line2,
                            Address3 = p.Family.Address.Line3,
                            Address4 = p.Family.Address.Line4,
                            Lat = p.Family.Address.Lat == null ? 0 : p.Family.Address.Lat,
                            Lng = p.Family.Address.Long == null ? 0 : p.Family.Address.Long,
                            RoleName = ((from r in p.PersonRoles select r).FirstOrDefault()).Role == null ? "Member" : ((from r in p.PersonRoles select r).FirstOrDefault()).Role.Name,
                            HasUsername = p.Username!=null,
                            FindFamily = false,
                            GroupId = 0,
                            Site = p.SiteId.HasValue ? p.Site.Name : "Select site...",
                            HeardAbout = p.PersonOptionalFields.Where<PersonOptionalField>(c => c.OptionalFieldId == (int)OptionalFields.HeardAbout).FirstOrDefault().Value
                        }).FirstOrDefault();

                
                //If the person is only part of one group - then that's his groupid
                if ((from pg in context.PersonGroups where pg.PersonId == personId select pg).Count() == 1)
                {
                    person.GroupId = (from pg in context.PersonGroups where pg.PersonId == personId select pg.GroupId).FirstOrDefault();
                }
                person.FamilyMembers = FetchFamilyMembers(personId, familyId, context);
                person.SecurityRoles = Cache.SecurityRoles(context, currentPerson);

                return person;
            }

        }

        public static List<FamilyMemberViewModel> AddPersonToFamily(int familyId, int personId)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var person = (from p in context.People
                              where p.PersonId == personId
                              select p).FirstOrDefault();

                person.FamilyId = familyId;
                person.Changed = DateTime.Now;
                context.SaveChanges();

                return FetchFamilyMembers(personId, familyId, context);
            }
        }


        public static AutoCompleteViewModel[] FetchFamilyAutoComplete(string term, int churchId)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {

                return (from f in context.Families
                        where f.ChurchId == churchId
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
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var query = (from p in context.People.Include("PersonOptionalField").Include("Address")
                             join pr in context.PersonRoles
                                on p.PersonId equals pr.PersonId
                             where p.ChurchId == currentPerson.ChurchId
                             select p);

                if (term.Contains(" "))
                {
                    string searchString1 = string.Empty;
                    string searchString2 = string.Empty;
                    string[] searchStrings = term.Split(' ');
                    searchString1 = searchStrings[0];
                    searchString2 = searchStrings[1];
                    query = query.Where(p => p.Firstname.Contains(searchString1) && p.Family.FamilyName.Contains(searchString2));
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

        public static int SavePerson(PersonViewModel person, Person currentPerson)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                if (!currentPerson.HasPermission(Permissions.EditChurchPersonalDetails))
                {
                    if (currentPerson.HasPermission(Permissions.EditGroupPersonalDetails))
                    {
                        if (!CheckSavePermissionGroup(person, currentPerson, context)) { return person.PersonId; }
                    }
                    else if (currentPerson.HasPermission(Permissions.EditOwnDetails))
                    {
                        if (!CheckSavePermissionPersonal(person, currentPerson, context)) { return person.PersonId; }
                    }
                    else
                    {
                        return person.PersonId;
                    }
                }

                bool sendWelcomeEmail;
                Church church;
                Person personToSave;

                GetPersonToSaveEntity(person, currentPerson, context, out sendWelcomeEmail, out church, out personToSave);
                bool anniversaryHasChanged = SavePersonalDetails(person, currentPerson, context, personToSave);
                SaveRole(person, currentPerson, context, personToSave);
                SaveContactInformation(person, personToSave);
                SaveAddressInformation(person, personToSave);
                UpdateRelationships(person, context, personToSave, anniversaryHasChanged);
                SendEmails(person, sendWelcomeEmail, church, personToSave);
                SavePerson(person, context, personToSave);
                EmailGroupLeader(person, currentPerson, context, church, personToSave);

                return personToSave.PersonId;
            }
        }

        public static void DeletePerson(int personId)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var optionalFields = (from pc in context.PersonOptionalFields
                                      where pc.PersonId == personId
                                      select pc);

                foreach (PersonOptionalField optionalField in optionalFields)
                {
                    context.DeleteObject(optionalField);
                }

                int tableId = (int)Tables.Person;

                var events = (from e in context.Events
                                      where e.Reference == personId
                                      && e.TableId == tableId
                                      select e);

                foreach (Event eventToDelete in events)
                {
                    context.DeleteObject(eventToDelete);
                }

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

                //Remove them from groups
                var groups = (from g in context.PersonGroups
                                where g.PersonId == personId
                                select g);

                foreach (PersonGroup personGroup in groups)
                {
                    context.DeleteObject(personGroup);
                }

                //Remove them from roles
                var personRoles = (from pr in context.PersonRoles
                                   where pr.PersonId == personId
                                   select pr);

                foreach (PersonRole personRole in personRoles)
                {
                    context.DeleteObject(personRole);
                }

                //Remove them from any leadership or admin roles in Home Groups
                var homeGroups = (from g in context.Groups
                                  where g.AdministratorId == personId ||
                                        g.LeaderId == personId
                                  select g);

                foreach (Group homeGroup in homeGroups)
                {
                    if (homeGroup.LeaderId == personId)
                        homeGroup.LeaderId = null;
                    if (homeGroup.AdministratorId == personId)
                        homeGroup.AdministratorId = null;
                }

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



                var person = (from p in context.People
                              where p.PersonId == personId
                              select p).FirstOrDefault();

                //Check to see if they were the last one in their family
                var familyMembers = (from p in context.People
                                     where p.PersonId != personId
                                     && p.FamilyId == person.FamilyId
                                     select p).ToList();

                context.DeleteObject(person);

                if (familyMembers.Count == 0)
                {
                    var familyToDelete = (from f in context.Families
                                          join p in context.People
                                            on f.FamilyId equals p.FamilyId
                                          where p.PersonId == personId
                                          select f).FirstOrDefault();

                    context.DeleteObject(familyToDelete);
                }

                context.SaveChanges();
            }

        }

        #region Private Methods
        private static void SavePerson(PersonViewModel person, oikonomosEntities context, Person personToSave)
        {
            if (person.PersonId == 0)
            {
                context.AddToPeople(personToSave);
            }

            context.SaveChanges();
        }

        private static void EmailGroupLeader(PersonViewModel person, Person currentPerson, oikonomosEntities context, Church church, Person personToSave)
        {
            if (personToSave.HasPermission(Permissions.NotifyGroupLeaderOfVisit) && person.GroupId > 0)
            {
                bool sendEmailToGroupLeader = person.PersonId == 0;
                var personGroup = (from pg in context.PersonGroups
                                   where pg.PersonId == personToSave.PersonId
                                   select pg).FirstOrDefault();

                if (personGroup == null)
                {
                    personGroup = AddPersonToGroup(person, context, personToSave, personGroup);
                }
                else if (personGroup.GroupId != person.GroupId)
                {
                    MovePersonToGroup(person, context, personGroup);
                    sendEmailToGroupLeader = true;
                }

                if (personGroup.Group.LeaderId == currentPerson.PersonId ||
                    personGroup.Group.AdministratorId == currentPerson.PersonId)
                {
                    sendEmailToGroupLeader = false;  //This is the groupleader
                }

                if (sendEmailToGroupLeader)
                {
                    //Send email to the home group leader
                    var group = (from g in context.Groups
                                 where g.GroupId == person.GroupId
                                 select g).FirstOrDefault();

                    if (group != null)
                    {
                        if (group.Leader != null && group.Leader.HasValidEmail() && group.LeaderId != currentPerson.PersonId)
                        {
                            Email.SendNewVisitorEmail(person, church, group.Leader.Firstname, group.Leader.Family.FamilyName, group.Leader.Email);
                        }
                        else if (group.Administrator != null && group.Administrator.HasValidEmail() && group.LeaderId != currentPerson.PersonId)
                        {
                            Email.SendNewVisitorEmail(person, church, group.Administrator.Firstname, group.Administrator.Family.FamilyName, group.Administrator.Email);
                        }
                    }
                }
            }
        }

        private static void SendEmails(PersonViewModel person, bool sendWelcomeEmail, Church church, Person personToSave)
        {
            if (sendWelcomeEmail && person.PersonId == 0 && personToSave.HasPermission(Permissions.SendWelcomeLetter) && personToSave.HasValidEmail())
            {
                SendVisitorWelcome(sendWelcomeEmail,
                    person.Firstname,
                    person.Surname,
                    church,
                    person.Email,
                    personToSave);
            }
            else if (sendWelcomeEmail && personToSave.HasValidEmail() && personToSave.HasPermission(Permissions.Login))
            {
                SendEmailAndPassword(person.Firstname,
                    person.Surname,
                    church,
                    person.Email,
                    personToSave);
            }
        }

        private static bool SavePersonalDetails(PersonViewModel person, Person currentPerson, oikonomosEntities context, Person personToSave)
        {
            bool anniversaryHasChanged = false;
            if (personToSave.Anniversary != person.Anniversary_Value)
            {
                anniversaryHasChanged = true;
            }
            personToSave.Anniversary = person.Anniversary_Value;

            personToSave.Occupation = person.Occupation;
            Site selectedSite = (from s in context.Sites where s.ChurchId == currentPerson.ChurchId && s.Name == person.Site select s).FirstOrDefault();
            if (selectedSite == null)
            {
                personToSave.SiteId = null;
            }
            else
            {
                personToSave.SiteId = selectedSite.SiteId;
                if (person.FamilyId > 0)
                {
                    var familyMembers = (from p in context.People
                                         where p.FamilyId == person.FamilyId
                                         && p.SiteId == null
                                         select p);
                    foreach (Person p in familyMembers)
                    {
                        p.SiteId = selectedSite.SiteId;
                    }
                }
            }

            personToSave.Family.Changed = DateTime.Now;
            personToSave.Changed = DateTime.Now;
            return anniversaryHasChanged;
        }

        private static void SaveRole(PersonViewModel person, Person currentPerson, oikonomosEntities context, Person personToSave)
        {
            int roleId = context
                .Roles
                .Where(r => (r.ChurchId == currentPerson.ChurchId && r.Name == person.RoleName))
                .FirstOrDefault()
                .RoleId;

            bool addRole = false;
            if (personToSave.PersonRoles.Count > 0 && personToSave.PersonRoles.FirstOrDefault().RoleId != roleId)
            {
                personToSave.PersonRoles.Remove(personToSave.PersonRoles.FirstOrDefault());
                addRole = true;
            }

            if (personToSave.PersonRoles.Count == 0 || addRole)
            {
                SavePersonRole(personToSave, roleId);
            }
        }

        private static void SavePersonRole(Person personToSave, int roleId)
        {
            PersonRole pr = new PersonRole();
            pr.RoleId = roleId;
            pr.Created = DateTime.Now;
            pr.Changed = DateTime.Now;
            personToSave.PersonRoles.Add(pr);
        }

        private static void MovePersonToGroup(PersonViewModel person, oikonomosEntities context, PersonGroup personGroup)
        {
            personGroup.GroupId = person.GroupId;
            context.SaveChanges();
        }

        private static PersonGroup AddPersonToGroup(PersonViewModel person, oikonomosEntities context, Person personToSave, PersonGroup personGroup)
        {
            personGroup = new PersonGroup();
            personGroup.GroupId = person.GroupId;
            personGroup.Person = personToSave;
            personGroup.Joined = DateTime.Now;
            personGroup.Created = DateTime.Now;
            personGroup.Changed = DateTime.Now;
            personToSave.PersonGroups.Add(personGroup);
            context.SaveChanges();
            return personGroup;
        }

        private static void UpdateRelationships(PersonViewModel person, oikonomosEntities context, Person personToSave, bool anniversaryHasChanged)
        {
            if (person.FamilyMembers != null)
            {
                foreach (FamilyMemberViewModel familyMember in person.FamilyMembers)
                {
                    if (familyMember.Relationship != null)
                    {
                        Relationships relationship = (Relationships)Enum.Parse(typeof(Relationships), familyMember.Relationship);
                        if (anniversaryHasChanged && (relationship == Relationships.Husband || relationship == Relationships.Wife))
                        {
                            var spouse = (from p in context.People
                                          where p.PersonId == familyMember.PersonId
                                          select p).FirstOrDefault();

                            if (spouse != null)
                            {
                                spouse.Anniversary = personToSave.Anniversary;
                            }
                        }

                        AddPersonRelationship(personToSave.PersonId, familyMember.PersonId, (int)relationship, personToSave, context);

                        //Check the opposite relationship
                        UpdateOtherRelationships(familyMember, person, context);
                    }
                }
            }
        }

        private static void SaveAddressInformation(PersonViewModel person, Person personToSave)
        {
            if (personToSave.Family.Address == null)
            {
                personToSave.Family.Address = new Address();
                personToSave.Family.Address.Created = DateTime.Now;
                personToSave.Family.Address.Changed = DateTime.Now;
            }

            if (personToSave.Family.Address.Line1 != person.Address1 ||
                personToSave.Family.Address.Line2 != person.Address2 ||
                personToSave.Family.Address.Line3 != person.Address3 ||
                personToSave.Family.Address.Line4 != person.Address4)
                personToSave.Family.Address.Changed = DateTime.Now;

            personToSave.Family.Address.Line1 = person.Address1 == null ? string.Empty : person.Address1;
            personToSave.Family.Address.Line2 = person.Address2 == null ? string.Empty : person.Address2;
            personToSave.Family.Address.Line3 = person.Address3 == null ? string.Empty : person.Address3;
            personToSave.Family.Address.Line4 = person.Address4 == null ? string.Empty : person.Address4;
            personToSave.Family.Address.Lat = person.Lat;
            personToSave.Family.Address.Long = person.Lng;
        }

        private static void SaveContactInformation(PersonViewModel person, Person personToSave)
        {
            personToSave.Family.HomePhone = person.HomePhone;
            UpdatePersonOptionalField(personToSave, OptionalFields.CellPhone, person.CellPhone);
            UpdatePersonOptionalField(personToSave, OptionalFields.Skype, person.Skype);
            UpdatePersonOptionalField(personToSave, OptionalFields.Twitter, person.Twitter);
            UpdatePersonOptionalField(personToSave, OptionalFields.WorkPhone, person.WorkPhone);
            UpdatePersonOptionalField(personToSave, OptionalFields.HeardAbout, person.HeardAbout);
            UpdatePersonOptionalField(personToSave, OptionalFields.Gender, person.Gender);
        }

        private static bool CheckSavePermissionPersonal(PersonViewModel person, Person currentPerson, oikonomosEntities context)
        {
            bool canSave = false;
            if (person.PersonId > 0)
            {
                var familyPerson = (from p in context.People
                                    where p.PersonId == person.PersonId
                                    && p.FamilyId == currentPerson.FamilyId
                                    select p).FirstOrDefault();
                if (familyPerson != null)
                {
                    canSave = true;
                }
            }
            else
            {
                canSave = true;

            }
            return canSave;
        }

        private static bool CheckSavePermissionGroup(PersonViewModel person, Person currentPerson, oikonomosEntities context)
        {
            bool canSave = false;
            if (person.PersonId > 0)
            {
                var groupPerson = (from pg in context.PersonGroups
                                   join g in context.Groups
                                   on pg.GroupId equals g.GroupId
                                   where pg.PersonId == person.PersonId
                                   && (g.LeaderId == currentPerson.PersonId || g.AdministratorId == currentPerson.PersonId)
                                   select pg).FirstOrDefault();
                if (groupPerson != null)
                {
                    canSave = true;
                }
            }
            else
            {
                canSave = true;
                if (!(person.RoleName == "Visitor" || person.RoleName == "Member"))
                {
                    person.RoleName = "Visitor";
                }
            }
            return canSave;
        }

        private static void GetPersonToSaveEntity(PersonViewModel person, Person currentPerson, oikonomosEntities context, out bool sendWelcomeEmail, out Church church, out Person personToSave)
        {
            sendWelcomeEmail = false;

            //We need some settings from the Church table
            church = (from c in context.Churches
                      where c.ChurchId == currentPerson.ChurchId
                      select c).FirstOrDefault();


            personToSave = new Person();
            if (person.PersonId != 0)
            {
                personToSave = (from p in context.People
                                where p.PersonId == person.PersonId
                                select p).FirstOrDefault();
            }
            else
            {
                personToSave.ChurchId = currentPerson.ChurchId;

                personToSave.Created = DateTime.Now;
                if (church.SendWelcome)
                {
                    sendWelcomeEmail = true;
                }

                if (person.GroupId > 0)
                {
                    PersonGroup pg = new PersonGroup();
                    pg.GroupId = person.GroupId;
                    pg.Person = personToSave;
                    pg.Joined = DateTime.Now;
                    pg.Created = DateTime.Now;
                    pg.Changed = DateTime.Now;
                    personToSave.PersonGroups.Add(pg);
                }
            }

            if (person.FamilyId == 0)
            {
                if (person.FindFamily)
                {
                    var family = (from f in context.Families
                                  join p in context.People
                                      on f.FamilyId equals p.FamilyId
                                  join g in context.PersonGroups
                                      on p.PersonId equals g.PersonId
                                  where f.FamilyName == person.Surname
                                      && g.GroupId == person.GroupId
                                  select f).FirstOrDefault();
                    if (family == null)
                    {
                        personToSave.Family = new Family();
                        personToSave.Family.Created = DateTime.Now;
                        personToSave.Family.ChurchId = currentPerson.ChurchId;
                    }
                    else
                    {
                        personToSave.Family = family;
                    }
                }
                else
                {
                    personToSave.Family = new Family();
                    personToSave.Family.Created = DateTime.Now;
                    personToSave.Family.ChurchId = currentPerson.ChurchId;
                }
            }
            else
            {
                personToSave.Family = (from f in context.Families
                                       where f.FamilyId == person.FamilyId
                                       select f).FirstOrDefault();

                if (personToSave.Family == null)
                {
                    personToSave.Family = new Family();
                    personToSave.Family.Created = DateTime.Now;
                    personToSave.Family.ChurchId = currentPerson.ChurchId;
                }
            }

            personToSave.Firstname = person.Firstname;
            personToSave.Family.FamilyName = person.Surname;
            personToSave.Email = person.Email;
            personToSave.DateOfBirth = person.DateOfBirth_Value;
        }

        private static void SendVisitorWelcome(bool includeUsername,
            string firstname,
            string surname,
            Church church,
            string email,
            Person personToSave)
        {
            string password = string.Empty;
            if (includeUsername)
            {
                personToSave.Username = (firstname + surname).Replace(" ", string.Empty);  //TODO replace with a boolean saying welcome letter has been sent
                password = RandomPasswordGenerator.Generate(RandomPasswordOptions.AlphaNumeric);
                personToSave.PasswordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");
            }
            personToSave.PublicId = Email.SendWelcomeEmail(firstname,
                  surname,
                  church,
                  email,
                  password,
                  true,
                  includeUsername);
        }

        private static void SendEmailAndPassword(string firstname,
            string surname,
            Church church,
            string email,
            Person personToSave)
        {
            personToSave.Username = (firstname + surname).Replace(" ", string.Empty);  //TODO replace with a boolean saying welcome letter has been sent
            string password = RandomPasswordGenerator.Generate(RandomPasswordOptions.AlphaNumeric);
            personToSave.PasswordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");
            personToSave.PublicId = Email.SendWelcomeEmail(firstname,
                  surname,
                  church,
                  email,
                  password,
                  false,
                  false);
        }

        private static void UpdateOtherRelationships(FamilyMemberViewModel familyMember, PersonViewModel person, oikonomosEntities context)
        {
            try
            {
                Relationships relationship = (Relationships)Enum.Parse(typeof(Relationships), familyMember.Relationship);
                Relationships oppositeRelationship = Relationships.Unknown;
                Person familyMemberToUpdate = (from p in context.People.Include("PersonRelationships")
                                         where p.PersonId == familyMember.PersonId
                                         select p).FirstOrDefault();

                bool isMale = (from pr in context.PersonRelationships
                               where pr.PersonRelatedToId == person.PersonId
                                    && (pr.RelationshipId == (int)Relationships.Husband ||
                                        pr.RelationshipId == (int)Relationships.Brother ||
                                        pr.RelationshipId == (int)Relationships.Father || 
                                        pr.RelationshipId == (int)Relationships.Grandfather ||
                                        pr.RelationshipId == (int)Relationships.Grandson ||
                                        pr.RelationshipId == (int)Relationships.Son)
                                    select pr).Count()>0;

                bool isFemale = (from pr in context.PersonRelationships
                                 where pr.PersonRelatedToId == person.PersonId
                                    && (pr.RelationshipId == (int)Relationships.Wife ||
                                        pr.RelationshipId == (int)Relationships.Sister ||
                                        pr.RelationshipId == (int)Relationships.Mother || 
                                        pr.RelationshipId == (int)Relationships.Grandmother ||
                                        pr.RelationshipId == (int)Relationships.Granddaughter ||
                                        pr.RelationshipId == (int)Relationships.Daughter)
                                    select pr).Count()>0;

                switch (relationship)
                {
                    case Relationships.Husband:
                        {
                            oppositeRelationship = Relationships.Wife;
                            break;
                        }
                    case Relationships.Wife:
                        {
                            oppositeRelationship = Relationships.Husband;
                            break;
                        }
                    case Relationships.Son:
                    case Relationships.Daughter:
                        {
                            if(isMale)
                            {
                            oppositeRelationship = Relationships.Father;
                            }

                            if(isFemale)
                            {
                                oppositeRelationship = Relationships.Mother;
                            }
                            break;
                        }
                    case Relationships.Brother:
                    case Relationships.Sister:
                        {
                            if(isMale)
                            {
                            oppositeRelationship = Relationships.Brother;
                            }

                            if(isFemale)
                            {
                                oppositeRelationship = Relationships.Sister;
                            }
                            break;
                        }
                    case Relationships.Father:
                    case Relationships.Mother:
                        {
                            if (isMale)
                            {
                                oppositeRelationship = Relationships.Son;
                            }

                            if (isFemale)
                            {
                                oppositeRelationship = Relationships.Daughter;
                            }
                            break;
                        }
                    case Relationships.Grandfather:
                    case Relationships.Grandmother:
                        {
                            if (isMale)
                            {
                                oppositeRelationship = Relationships.Grandson;
                            }

                            if (isFemale)
                            {
                                oppositeRelationship = Relationships.Granddaughter;
                            }
                            break;
                        }
                    case Relationships.Grandson:
                    case Relationships.Granddaughter:
                        {
                            if (isMale)
                            {
                                oppositeRelationship = Relationships.Grandfather;
                            }

                            if (isFemale)
                            {
                                oppositeRelationship = Relationships.Grandmother;
                            }
                            break;
                        }
                }

                if (oppositeRelationship != Relationships.Unknown)
                {
                    AddPersonRelationship(familyMember.PersonId, person.PersonId, (int)oppositeRelationship, familyMemberToUpdate, context);

                    var personToUpdate = (from p in context.People
                                          where p.PersonId == person.PersonId
                                          select p).FirstOrDefault();
                    //What about the rest of the family
                    if (relationship == Relationships.Husband || relationship == Relationships.Wife)
                    {
                        var spouseRelationships = (from pr in context.PersonRelationships
                                                    where pr.PersonId == familyMember.PersonId
                                                    && (pr.RelationshipId == (int)Relationships.Son ||
                                                        pr.RelationshipId == (int)Relationships.Daughter ||
                                                        pr.RelationshipId == (int)Relationships.Grandson ||
                                                        pr.RelationshipId == (int)Relationships.Granddaughter)
                                                    select pr);

                        foreach (PersonRelationship pr in spouseRelationships)
                        {
                            if (person.PersonId != pr.PersonRelatedToId)
                            {
                                AddPersonRelationship(person.PersonId, pr.PersonRelatedToId, pr.RelationshipId, personToUpdate, context);
                            }
                        }

                    }

                    if (relationship == Relationships.Brother || relationship == Relationships.Sister)
                    {
                        //He has a brother - does the brother have a father, grandfather, mother etc
                        var siblingRelationships = (from pr in context.PersonRelationships
                                                    where pr.PersonId == familyMember.PersonId
                                                    && (pr.RelationshipId == (int)Relationships.Father ||
                                                        pr.RelationshipId == (int)Relationships.Mother ||
                                                        pr.RelationshipId == (int)Relationships.Grandfather ||
                                                        pr.RelationshipId == (int)Relationships.Grandmother ||
                                                        pr.RelationshipId == (int)Relationships.Sister ||
                                                        pr.RelationshipId == (int)Relationships.Brother)
                                                    select pr);
                        
                        foreach (PersonRelationship pr in siblingRelationships)
                        {
                            if (person.PersonId != pr.PersonRelatedToId)
                            {
                                AddPersonRelationship(personToUpdate.PersonId, pr.PersonRelatedToId, pr.RelationshipId, personToUpdate, context);
                            }
                        }

                    }

                    //if (relationship == Relationships.Father || relationship == Relationships.Mother)
                    //{
                    //    //He has a brother - does the brother have a father, grandfather, mother etc
                    //    var siblingRelationships = (from pr in context.PersonRelationships
                    //                                where pr.PersonId == familyMember.PersonId
                    //                                && (pr.RelationshipId == (int)Relationships.Father ||
                    //                                    pr.RelationshipId == (int)Relationships.Mother ||
                    //                                    pr.RelationshipId == (int)Relationships.Grandfather ||
                    //                                    pr.RelationshipId == (int)Relationships.Grandmother ||
                    //                                    pr.RelationshipId == (int)Relationships.Sister ||
                    //                                    pr.RelationshipId == (int)Relationships.Brother)
                    //                                select pr);

                    //    foreach (PersonRelationship pr in siblingRelationships)
                    //    {
                    //        AddPersonRelationship(personToUpdate.PersonId, pr.PersonRelatedToId, pr.RelationshipId, personToUpdate, context);
                    //    }

                    //}
                }

            }
            catch { }
        }

        private static void AddPersonRelationship(int personId, int personRelatedToId, int relationshipId, Person familyMemberToUpdate, oikonomosEntities context)
        {
            bool found = false;
            foreach (PersonRelationship rel in familyMemberToUpdate.PersonRelationships)
            {
                //Update the existing one
                if (rel.PersonRelatedToId == personRelatedToId)
                {
                    if (rel.RelationshipId != relationshipId)
                    {
                        rel.RelationshipId = relationshipId;
                        rel.Changed = DateTime.Now;
                    }
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                //Create a new one
                PersonRelationship newRelationship = new PersonRelationship();
                newRelationship.PersonRelatedToId = personRelatedToId;
                newRelationship.RelationshipId = relationshipId;
                newRelationship.PersonId = personId;
                newRelationship.Created = DateTime.Now;
                newRelationship.Changed = DateTime.Now;
                context.AddToPersonRelationships(newRelationship);
            }
        }

        private static List<FamilyMemberViewModel> FetchFamilyMembers(int personId, int familyId, oikonomosEntities context)
        {
            var personFirstname = (from p in context.People
                                   where p.PersonId == personId
                                   select p.Firstname).FirstOrDefault();
            var familyMembers = (
                    from p in context.People
                    join f in context.Families
                    on p.FamilyId equals f.FamilyId
                    where f.FamilyId == familyId
                    && p.PersonId != personId
                    select new FamilyMemberViewModel
                    {
                        PersonId = p.PersonId,
                        FamilyMember = p.Firstname,
                        Person = personFirstname,
                        Relationship = (from pr in context.PersonRelationships
                                        where pr.PersonId == personId
                                        && pr.PersonRelatedToId == p.PersonId
                                        select pr.Relationship.Name).FirstOrDefault(),
                        RelationshipId = (from pr in context.PersonRelationships
                                          where pr.PersonId == personId
                                          && pr.PersonRelatedToId == p.PersonId
                                          select pr.RelationshipId).FirstOrDefault(),
                        FacebookId = p.PersonOptionalFields.Where<PersonOptionalField>(c => c.OptionalFieldId == (int)OptionalFields.Facebook).FirstOrDefault() == null ? "" : p.PersonOptionalFields.Where<PersonOptionalField>(c => c.OptionalFieldId == (int)OptionalFields.Facebook).FirstOrDefault().Value
                    }).ToList();

            familyMembers.Sort(delegate(FamilyMemberViewModel e1, FamilyMemberViewModel e2)
            {
                return e1.RelationshipId.CompareTo(e2.RelationshipId);
            });

            return familyMembers;
        }

        private static void UpdatePersonOptionalField(Person person, OptionalFields optionalField, string value)
        {
            PersonOptionalField personOptionalField = person.PersonOptionalFields.Where<PersonOptionalField>(c => c.OptionalFieldId == (int)optionalField).FirstOrDefault();
            if (personOptionalField == null)
            {
                personOptionalField = new PersonOptionalField();
                personOptionalField.OptionalFieldId = (int)optionalField;
                personOptionalField.Created = DateTime.Now;
                personOptionalField.Changed = DateTime.Now;
                person.PersonOptionalFields.Add(personOptionalField);
            }
            if (personOptionalField.Value != (value == null ? string.Empty : value))
                personOptionalField.Changed = DateTime.Now;
            personOptionalField.Value = value==null?string.Empty:value;
            
        }

        private static Person CheckEmailPassword(string email, string password, oikonomosEntities context)
        {
            string passwordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");

            var person= (from p in context.People.Include("PersonRoles")
                    join pr in context.PersonRoles
                        on p.PersonId equals pr.PersonId
                    join permissions in context.PermissionRoles
                        on pr.RoleId equals permissions.RoleId
                    where p.Email == email
                        && p.PasswordHash == passwordHash
                        && permissions.PermissionId == (int)Permissions.Login
                    select p).FirstOrDefault();
            if (person != null)
            {
                bool isSysAdmin = person.HasPermission(Permissions.SystemAdministrator); //Work around because Eager loading is not working???
            }
            return person;
        }

        private static Person CheckEmailPassword(int personId, string password, oikonomosEntities context)
        {
            string passwordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");

            return (from p in context.People
                    where p.PersonId == personId
                    && p.PasswordHash == passwordHash
                    select p).FirstOrDefault();
        }
        #endregion Private Methods
    }
}