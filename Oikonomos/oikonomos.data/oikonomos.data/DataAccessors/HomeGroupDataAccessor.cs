using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using oikonomos.data;
using oikonomos.common;
using System.Configuration;
using oikonomos.common.Models;
using Lib.Web.Mvc.JQuery.JqGrid;
using oikonomos.data.Services;
using System.Data.Objects;

namespace oikonomos.data.DataAccessors
{
    public static class GroupDataAccessor
    {
        public static GridSetupViewModel FetchGroupAttendanceGridSetup()
        {
            GridSetupViewModel model = new GridSetupViewModel();
            model.colNames = new List<string>();
            model.colNames.Add("Group Name");
            for (int monthCount = -5; monthCount <= 0; monthCount++)
            {
                model.colNames.Add(DateTime.Now.AddMonths(monthCount).ToString("MMM"));
            }

            model.colModel = new List<GridColModel>();
            model.colModel.Add(new GridColModel());
            model.colModel[0].index = "Name";
            model.colModel[0].name = "Name";
            model.colModel[0].align = "left";
            model.colModel[0].search = true;
            model.colModel[0].width = 150;

            for (int col = 1; col <= 6; col++)
            {
                model.colModel.Add(new GridColModel());
                model.colModel[col].index = DateTime.Now.AddMonths(col - 6).ToString("MMM");
                model.colModel[col].name = DateTime.Now.AddMonths(col - 6).ToString("MMM");
                model.colModel[col].align = "center";
                model.colModel[col].search = false;
                model.colModel[col].width = 60;
            }

            return model;
        }

        public static JqGridData FetchGroupAttendanceJQGrid(Person currentPerson, JqGridRequest request)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                JqGridData attendanceGridData = new JqGridData();

                ObjectResult<FetchGroupAttendance_Result> results = context.FetchGroupAttendance(currentPerson.ChurchId);
                var toSort = results.AsEnumerable();

                switch (request.sidx)
                {
                    case "Name":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                toSort = toSort.OrderBy(g => g.Name).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                toSort = toSort.OrderByDescending(g => g.Name).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "Jan":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                toSort = toSort.OrderBy(g => g.C1).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                toSort = toSort.OrderByDescending(g => g.C1).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "Feb":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                toSort = toSort.OrderBy(g => g.C2).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                toSort = toSort.OrderByDescending(g => g.C2).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "Mar":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                toSort = toSort.OrderBy(g => g.C3).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                toSort = toSort.OrderByDescending(g => g.C3).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "Apr":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                toSort = toSort.OrderBy(g => g.C4).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                toSort = toSort.OrderByDescending(g => g.C4).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "May":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                toSort = toSort.OrderBy(g => g.C5).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                toSort = toSort.OrderByDescending(g => g.C5).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "Jun":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                toSort = toSort.OrderBy(g => g.C6).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                toSort = toSort.OrderByDescending(g => g.C6).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "Jul":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                toSort = toSort.OrderBy(g => g.C7).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                toSort = toSort.OrderByDescending(g => g.C7).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "Aug":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                toSort = toSort.OrderBy(g => g.C8).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                toSort = toSort.OrderByDescending(g => g.C8).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "Sep":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                toSort = toSort.OrderBy(g => g.C9).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                toSort = toSort.OrderByDescending(g => g.C9).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "Oct":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                toSort = toSort.OrderBy(g => g.C10).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                toSort = toSort.OrderByDescending(g => g.C10).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "Nov":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                toSort = toSort.OrderBy(g => g.C11).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                toSort = toSort.OrderByDescending(g => g.C11).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "Dec":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                toSort = toSort.OrderBy(g => g.C12).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                toSort = toSort.OrderByDescending(g => g.C12).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                }

                int currentMonth = DateTime.Now.Month;
                int totalRecords = (from g in context.Groups
                                    where g.ChurchId == currentPerson.ChurchId
                                    select g).Count();

                JqGridData sitesGridData = new JqGridData()
                {
                    total = (int)Math.Ceiling((float)totalRecords / (float)request.rows),
                    page = request.page,
                    records = totalRecords,
                    rows = (from g in toSort
                            select new JqGridRow()
                            {
                                id = g.GroupId.ToString(),
                                cell = new string[] {
                                                    g.Name,
                                                    currentMonth==1 ? (g.C8.HasValue ? (g.C8.Value*100M).ToString("0") + "%" : "nc") : currentMonth==2 ? (g.C9.HasValue ? (g.C9.Value*100M).ToString("0") + "%" : "nc") : currentMonth==3 ? (g.C10.HasValue ? (g.C10.Value*100M).ToString("0") + "%" : "nc") : currentMonth==4 ? (g.C11.HasValue ? (g.C11.Value*100M).ToString("0") + "%" : "nc") : currentMonth==5 ? (g.C12.HasValue ? (g.C12.Value*100M).ToString("0") + "%" : "nc") : currentMonth==6 ? (g.C1.HasValue ? (g.C1.Value*100M).ToString("0") + "%" : "nc") : currentMonth==7 ? (g.C2.HasValue ? (g.C2.Value*100M).ToString("0") + "%" : "nc") : currentMonth==8 ? (g.C3.HasValue ? (g.C3.Value*100M).ToString("0") + "%" : "nc") : currentMonth==9 ? (g.C4.HasValue ? (g.C4.Value*100M).ToString("0") + "%" : "nc") : currentMonth==10 ? (g.C5.HasValue ? (g.C5.Value*100M).ToString("0") + "%" : "nc") : currentMonth==11 ? (g.C6.HasValue ? (g.C6.Value*100M).ToString("0") + "%" : "nc") : currentMonth==12 ? (g.C7.HasValue ? (g.C7.Value*100M).ToString("0") + "%" : "nc") : "0",
                                                    currentMonth==1 ? (g.C9.HasValue ? (g.C9.Value*100M).ToString("0") + "%" : "nc") : currentMonth==2 ? (g.C10.HasValue ? (g.C10.Value*100M).ToString("0") + "%" : "nc") : currentMonth==3 ? (g.C11.HasValue ? (g.C11.Value*100M).ToString("0") + "%" : "nc") : currentMonth==4 ? (g.C12.HasValue ? (g.C12.Value*100M).ToString("0") + "%" : "nc") : currentMonth==5 ? (g.C1.HasValue ? (g.C1.Value*100M).ToString("0") + "%" : "nc") : currentMonth==6 ? (g.C2.HasValue ? (g.C2.Value*100M).ToString("0") + "%" : "nc") : currentMonth==7 ? (g.C3.HasValue ? (g.C3.Value*100M).ToString("0") + "%" : "nc") : currentMonth==8 ? (g.C4.HasValue ? (g.C4.Value*100M).ToString("0") + "%" : "nc") : currentMonth==9 ? (g.C5.HasValue ? (g.C5.Value*100M).ToString("0") + "%" : "nc") : currentMonth==10 ? (g.C6.HasValue ? (g.C6.Value*100M).ToString("0") + "%" : "nc") : currentMonth==11 ? (g.C7.HasValue ? (g.C7.Value*100M).ToString("0") + "%" : "nc") : currentMonth==12 ? (g.C8.HasValue ? (g.C8.Value*100M).ToString("0") + "%" : "nc") : "0",
                                                    currentMonth==1 ? (g.C10.HasValue ? (g.C10.Value*100M).ToString("0") + "%" : "nc") : currentMonth==2 ? (g.C11.HasValue ? (g.C11.Value*100M).ToString("0") + "%" : "nc") : currentMonth==3 ? (g.C12.HasValue ? (g.C12.Value*100M).ToString("0") + "%" : "nc") : currentMonth==4 ? (g.C1.HasValue ? (g.C1.Value*100M).ToString("0") + "%" : "nc") : currentMonth==5 ? (g.C2.HasValue ? (g.C2.Value*100M).ToString("0") + "%" : "nc") : currentMonth==6 ? (g.C3.HasValue ? (g.C2.Value*100M).ToString("0") + "%" : "nc") : currentMonth==7 ? (g.C4.HasValue ? (g.C4.Value*100M).ToString("0") + "%" : "nc") : currentMonth==8 ? (g.C5.HasValue ? (g.C5.Value*100M).ToString("0") + "%" : "nc") : currentMonth==9 ? (g.C6.HasValue ? (g.C6.Value*100M).ToString("0") + "%" : "nc") : currentMonth==10 ? (g.C7.HasValue ? (g.C7.Value*100M).ToString("0") + "%" : "nc") : currentMonth==11 ? (g.C8.HasValue ? (g.C8.Value*100M).ToString("0") + "%" : "nc") : currentMonth==12 ? (g.C9.HasValue ? (g.C9.Value*100M).ToString("0") + "%" : "nc") : "0",
                                                    currentMonth==1 ? (g.C11.HasValue ? (g.C11.Value*100M).ToString("0") + "%" : "nc") : currentMonth==2 ? (g.C12.HasValue ? (g.C12.Value*100M).ToString("0") + "%" : "nc") : currentMonth==3 ? (g.C1.HasValue ? (g.C1.Value*100M).ToString("0") + "%" : "nc") : currentMonth==4 ? (g.C2.HasValue ? (g.C2.Value*100M).ToString("0") + "%" : "nc") : currentMonth==5 ? (g.C3.HasValue ? (g.C3.Value*100M).ToString("0") + "%" : "nc") : currentMonth==6 ? (g.C4.HasValue ? (g.C2.Value*100M).ToString("0") + "%" : "nc") : currentMonth==7 ? (g.C5.HasValue ? (g.C5.Value*100M).ToString("0") + "%" : "nc") : currentMonth==8 ? (g.C6.HasValue ? (g.C6.Value*100M).ToString("0") + "%" : "nc") : currentMonth==9 ? (g.C7.HasValue ? (g.C7.Value*100M).ToString("0") + "%" : "nc") : currentMonth==10 ? (g.C8.HasValue ? (g.C8.Value*100M).ToString("0") + "%" : "nc") : currentMonth==11 ? (g.C9.HasValue ? (g.C9.Value*100M).ToString("0") + "%" : "nc") : currentMonth==12 ? (g.C10.HasValue ? (g.C10.Value*100M).ToString("0") + "%" : "nc") : "0",
                                                    currentMonth==1 ? (g.C12.HasValue ? (g.C12.Value*100M).ToString("0") + "%" : "nc") : currentMonth==2 ? (g.C1.HasValue ? (g.C1.Value*100M).ToString("0") + "%" : "nc") : currentMonth==3 ? (g.C2.HasValue ? (g.C2.Value*100M).ToString("0") + "%" : "nc") : currentMonth==4 ? (g.C3.HasValue ? (g.C3.Value*100M).ToString("0") + "%" : "nc") : currentMonth==5 ? (g.C4.HasValue ? (g.C4.Value*100M).ToString("0") + "%" : "nc") : currentMonth==6 ? (g.C5.HasValue ? (g.C2.Value*100M).ToString("0") + "%" : "nc") : currentMonth==7 ? (g.C6.HasValue ? (g.C6.Value*100M).ToString("0") + "%" : "nc") : currentMonth==8 ? (g.C7.HasValue ? (g.C7.Value*100M).ToString("0") + "%" : "nc") : currentMonth==9 ? (g.C8.HasValue ? (g.C8.Value*100M).ToString("0") + "%" : "nc") : currentMonth==10 ? (g.C9.HasValue ? (g.C9.Value*100M).ToString("0") + "%" : "nc") : currentMonth==11 ? (g.C10.HasValue ? (g.C10.Value*100M).ToString("0") + "%" : "nc") : currentMonth==12 ? (g.C11.HasValue ? (g.C11.Value*100M).ToString("0") + "%" : "nc") : "0",
                                                    currentMonth==1 ? (g.C1.HasValue ? (g.C1.Value*100M).ToString("0") + "%" : "nc") : currentMonth==2 ? (g.C2.HasValue ? (g.C2.Value*100M).ToString("0") + "%" : "nc") : currentMonth==3 ? (g.C3.HasValue ? (g.C3.Value*100M).ToString("0") + "%" : "nc") : currentMonth==4 ? (g.C4.HasValue ? (g.C4.Value*100M).ToString("0") + "%" : "nc") : currentMonth==5 ? (g.C5.HasValue ? (g.C5.Value*100M).ToString("0") + "%" : "nc") : currentMonth==6 ? (g.C6.HasValue ? (g.C2.Value*100M).ToString("0") + "%" : "nc") : currentMonth==7 ? (g.C7.HasValue ? (g.C7.Value*100M).ToString("0") + "%" : "nc") : currentMonth==8 ? (g.C8.HasValue ? (g.C8.Value*100M).ToString("0") + "%" : "nc") : currentMonth==9 ? (g.C9.HasValue ? (g.C9.Value*100M).ToString("0") + "%" : "nc") : currentMonth==10 ? (g.C10.HasValue ? (g.C10.Value*100M).ToString("0") + "%" : "nc") : currentMonth==11 ? (g.C11.HasValue ? (g.C11.Value*100M).ToString("0") + "%" : "nc") : currentMonth==12 ? (g.C12.HasValue ? (g.C12.Value*100M).ToString("0") + "%" : "nc") : "0"
                                }
                            }).ToArray()
                };
                return sitesGridData;
            }
        }

        public static JqGridData FetchHomeGroupsJQGrid(Person currentPerson, JqGridRequest request, int selectedGroupId, bool useGroupId)
        {
            if (!(currentPerson.HasPermission(Permissions.EditOwnGroups) || currentPerson.HasPermission(Permissions.EditAllGroups)))
            {
                throw new Exception("Invalid security Role");
            }

            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var rules = request.filters == null ? null : request.filters.rules;
                var groups = FetchGroupList(currentPerson, request._search, rules, context);

                var sortedGroups = sortGroupList(request, groups).ToList();

                if (useGroupId)
                {
                    if (selectedGroupId == 0)
                        request.page = 1;
                    else
                    {
                        var result = sortedGroups
                            .Select((x, i) => new {Item = x, Index = i})
                            .FirstOrDefault(itemWithIndex => itemWithIndex.Item.GroupId == selectedGroupId);

                        request.page = 1;
                        if (result != null)
                            request.page = ((result.Index + request.rows)/request.rows);

                    }
                }

                var totalRecords = sortedGroups.Count();
                var filteredGroups = sortedGroups.Skip((request.page - 1) * request.rows).Take(request.rows).ToList();
                
                var sitesGridData = new JqGridData()
                {
                    total = (int)Math.Ceiling((float)totalRecords / (float)request.rows),
                    page = request.page,
                    records = totalRecords,
                    rows = (from g in filteredGroups.AsEnumerable()
                            select new JqGridRow()
                            {
                                id = g.GroupId.ToString(),
                                cell = new string[] {
                                                    g.GroupId.ToString(),
                                                    g.Name,
                                                    g.Leader==null? string.Empty : g.Leader.Firstname + " " + g.Leader.Family.FamilyName,
                                                    g.Administrator==null? string.Empty : g.Administrator.Firstname + " " + g.Administrator.Family.FamilyName,
                                                    g.Address==null? string.Empty : g.Address.ChurchSuburb==null?g.Address.Line3:g.Address.ChurchSuburb.Suburb,
                                                    g.GroupClassification==null? string.Empty :g.GroupClassification.Name,
                                                    g.PersonLinkedToGroups.FirstOrDefault(p => p.Description == CacheNames.OverseeingElder)==null? string.Empty : g.PersonLinkedToGroups.First(p => p.Description == CacheNames.OverseeingElder).Person.Firstname + " " + g.PersonLinkedToGroups.First(p => p.Description == CacheNames.OverseeingElder).Person.Family.FamilyName
                                }
                            }).ToArray()
                };


                return sitesGridData;
            }
        }

        private static IEnumerable<Group> sortGroupList(JqGridRequest request, IQueryable<Group> groups)
        {
            switch (request.sidx)
            {
                case "GroupName":
                    {
                        return request.sord.ToLower() == "asc"
                                     ? groups.OrderBy(g => g.Name)
                                     : groups.OrderByDescending(g => g.Name);
                    }
                case "LeaderName":
                    {
                        return request.sord.ToLower() == "asc"
                                     ? groups.OrderBy(g => g.Leader.Firstname)
                                     : groups.OrderByDescending(g => g.Leader.Firstname);
                    }
                case "Administrator":
                    {
                        return request.sord.ToLower() == "asc"
                                     ? groups.OrderBy(g => g.Administrator.Firstname)
                                     : groups.OrderByDescending(g => g.Administrator.Firstname);
                    }
                case "Suburb":
                    {
                        return request.sord.ToLower() == "asc"
                                     ? groups.OrderBy(g => g.Address.ChurchSuburb.Suburb).ThenBy(g => g.Address.Line3)
                                     : groups.OrderByDescending(g => g.Address.ChurchSuburb.Suburb)
                                             .ThenByDescending(g => g.Address.Line3);
                    }
                case "GroupClassification":
                    {
                        return request.sord.ToLower() == "asc"
                                     ? groups.OrderBy(g => g.GroupClassification.Name)
                                     : groups.OrderByDescending(g => g.GroupClassification.Name);
                    }
                case "OverseeingElder":
                    {
                        return request.sord.ToLower() == "asc"
                                     ? groups.OrderBy(g => g.PersonLinkedToGroups.FirstOrDefault(p => p.Description == CacheNames.OverseeingElder).Person.Firstname).ThenBy(g => g.PersonLinkedToGroups.FirstOrDefault(p => p.Description == CacheNames.OverseeingElder).Person.Family.FamilyName)
                                     : groups.OrderByDescending(g => g.PersonLinkedToGroups.FirstOrDefault(p => p.Description == CacheNames.OverseeingElder).Person.Firstname).ThenByDescending(g => g.PersonLinkedToGroups.FirstOrDefault(p => p.Description == CacheNames.OverseeingElder).Person.Family.FamilyName);
                    }
            }
            throw new Exception("Invalid sort parameter");
        }

        public static JqGridData FetchPeopleNotInAHomeGroupJQGrid(Person currentPerson, JqGridRequest request)
        {
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var includedRoles = context
                                    .PermissionRoles
                                    .Where(pr => pr.PermissionId == (int)Permissions.IncludeInNotInGroupList)
                                    .Select(pr => pr.RoleId)
                                    .ToList();


                var peopleNotInGroups = (from p in context.People
                                         join c in context.PersonChurches
                                           on p.PersonId equals c.PersonId
                                         join pg in context.PersonGroups
                                           on p.PersonId equals pg.PersonId into tList
                                         from pgEmpty in tList.DefaultIfEmpty()
                                         where pgEmpty.GroupId == null
                                         && c.ChurchId == currentPerson.ChurchId
                                         && includedRoles.Contains(c.RoleId)
                                         select p);

                var totalRecords = peopleNotInGroups.Count();

                switch (request.sidx)
                {
                    case "Firstname":
                    {
                        peopleNotInGroups = request.sord.ToLower() == "asc" ? peopleNotInGroups.OrderBy(p => p.Firstname).Skip((request.page - 1) * request.rows).Take(request.rows) : peopleNotInGroups.OrderByDescending(p => p.Firstname).Skip((request.page - 1) * request.rows).Take(request.rows);
                        break;
                    }
                    case "Surname":
                    {
                        peopleNotInGroups = request.sord.ToLower() == "asc" ? peopleNotInGroups.OrderBy(p => p.Family.FamilyName).Skip((request.page - 1) * request.rows).Take(request.rows) : peopleNotInGroups.OrderByDescending(p => p.Family.FamilyName).Skip((request.page - 1) * request.rows).Take(request.rows);
                        break;
                    }
                    case "Site":
                    {
                        peopleNotInGroups = request.sord.ToLower() == "asc" ? peopleNotInGroups.OrderBy(p => p.Site.Name).Skip((request.page - 1) * request.rows).Take(request.rows) : peopleNotInGroups.OrderByDescending(p => p.Site.Name).Skip((request.page - 1) * request.rows).Take(request.rows);
                        break;
                    }
                }

                var sitesGridData = new JqGridData()
                {
                    total = (int)Math.Ceiling((float)totalRecords / (float)request.rows),
                    page = request.page,
                    records = totalRecords,
                    rows = (from p in peopleNotInGroups.AsEnumerable()
                            select new JqGridRow()
                            {
                                id = p.PersonId.ToString(),
                                cell = new string[] {
                                                    p.PersonId.ToString(CultureInfo.InvariantCulture),
                                                    p.Firstname,
                                                    p.Family.FamilyName,
                                                    p.Family.HomePhone,
                                                    p.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int)OptionalFields.CellPhone)==null?"":p.PersonOptionalFields.Where<PersonOptionalField>(c => c.OptionalFieldId == (int)OptionalFields.CellPhone).FirstOrDefault().Value,
                                                    p.Email,
                                                    p.Site==null?string.Empty:p.Site.Name
                                }
                            }).ToArray()
                };
                return sitesGridData;
            }
        }


        public static IEnumerable<PersonListViewModel> FetchPeopleNotInAGroup(Person currentPerson)
        {
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var includedRoles = context
                    .PermissionRoles
                    .Where(pr => pr.PermissionId == (int)Permissions.IncludeInNotInGroupList)
                    .Select(pr => pr.RoleId)
                    .ToList();

                return (from p in context.People
                    join c in context.PersonChurches
                        on p.PersonId equals c.PersonId
                    join pg in context.PersonGroups
                        on p.PersonId equals pg.PersonId into tList
                    from pgEmpty in tList.DefaultIfEmpty()
                    where pgEmpty.GroupId == null
                          && c.ChurchId == currentPerson.ChurchId
                          && includedRoles.Contains(c.RoleId)
                    orderby p.Family.FamilyName, p.PersonId
                    select new PersonListViewModel
                    {
                        PersonId = p.PersonId,
                        FamilyId = p.FamilyId,
                        Firstname = p.Firstname,
                        Surname = p.Family.FamilyName,
                        HomePhone = p.Family.HomePhone,
                        CellPhone = p.PersonOptionalFields.FirstOrDefault(cp => cp.OptionalFieldId == (int) OptionalFields.CellPhone).Value,
                        WorkPhone = p.PersonOptionalFields.FirstOrDefault(cp => cp.OptionalFieldId == (int) OptionalFields.WorkPhone).Value,
                        Email = p.Email,
                        Site = p.Site == null ? string.Empty : p.Site.Name
                    }).ToList();

            }
        }

        public static string FetchHomeGroupName(int groupId)
        {
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                return (from g in context.Groups
                        where g.GroupId == groupId
                        select g.Name).FirstOrDefault();
            }
        }

        public static string FetchHomeGroupName(Person currentPerson, out bool displayList, ref int? groupId)
        {
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var homeGroups = (from g in context.Groups
                                  where (g.LeaderId == currentPerson.PersonId || g.AdministratorId == currentPerson.PersonId)
                                  && g.ChurchId==currentPerson.ChurchId
                                  orderby g.Name
                                  select g);
                if (!homeGroups.Any())
                {
                    displayList = false;
                    return string.Empty;
                }

                displayList = homeGroups.Count() != 1;

                if(!groupId.HasValue)
                    groupId = homeGroups.First().GroupId;
                return homeGroups.First().Name;
            }
        }

        public static bool DeleteHomeGroup(int groupId, bool confirmDelete, ref string message)
        {
            var success = false;
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {

                var rolesToInclude = context.PermissionRoles.Where(p => p.PermissionId == (int) Permissions.IncludeInChurchList).Select(r => r.RoleId);

                var peopleInRoles = context.PersonChurches.Where(r => rolesToInclude.Contains(r.RoleId)).Select(p => p.PersonId);

                    //Check to see if there is anyone in the group
                var peopleInGroup = (from pg in context.PersonGroups
                                     where pg.GroupId == groupId && peopleInRoles.Contains(pg.PersonId)
                                     select pg).Count();

                if (peopleInGroup == 0 || confirmDelete)
                {
                    //Delete any remaining people in the group
                    var peopleLeftInGroup = (from pg in context.PersonGroups
                                             where pg.GroupId == groupId
                                             select pg);
                    
                    foreach (var pg in peopleLeftInGroup)
                    {
                        context.PersonGroups.DeleteObject(pg);
                    }
                    
                    //Delete group
                    var groupToDelete = (from g in context.Groups
                                         where g.GroupId == groupId
                                         select g).FirstOrDefault();
                    if (groupToDelete != null)
                    {
                        var linkedPersonToDelete = context.PersonLinkedToGroups.FirstOrDefault(p => p.GroupId == groupToDelete.GroupId && p.Description == CacheNames.OverseeingElder);
                        if (linkedPersonToDelete != null)
                            context.PersonLinkedToGroups.DeleteObject(linkedPersonToDelete);
                        
                        context.Groups.DeleteObject(groupToDelete);
                        context.SaveChanges();
                        message = "Group succesfully deleted";
                        success = true;
                    }
                    else
                    {
                        message = "No group found to delete";
                    }
                }
                else
                {
                    message = "Are you sure you want to delete this group, there are still people in the group?";
                }
            }

            return success;
        }

        public static void SetHomeGroupLeader(Person currentPerson, int groupId, int leaderId)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                if (currentPerson.HasPermission(Permissions.SetGroupLeaderOrAdministrator))
                {
                    //Fetch the record
                    var groupToUpdate = (from g in context.Groups
                                         where g.GroupId == groupId
                                         && g.ChurchId == currentPerson.ChurchId
                                         select g).FirstOrDefault();
                    if (groupToUpdate != null)
                    {
                        groupToUpdate.LeaderId = leaderId;
                        context.SaveChanges();
                    }
                }
            }
        }

        public static void SetHomeGroupAdministrator(Person currentPerson, int groupId, int administratorId)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                if (currentPerson.HasPermission(Permissions.SetGroupLeaderOrAdministrator))
                {
                    //Fetch the record
                    var groupToUpdate = (from g in context.Groups
                                         where g.GroupId == groupId
                                         && g.ChurchId == currentPerson.ChurchId
                                         select g).FirstOrDefault();
                    if (groupToUpdate != null)
                    {
                        groupToUpdate.AdministratorId = administratorId;
                        context.SaveChanges();
                    }
                }
            }
        }

        public static List<string> FetchGroupLeaderAddresses(Person currentPerson, bool search, JqGridFilters filters, bool includeMembers)
        {
            List<string> addresses = new List<string>();
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                List<JqGridFilterRule> rules = filters == null ? null : filters.rules;
                var groups = FetchGroupList(currentPerson, search, rules, context);

                var leaders = (from p in context.People
                               join c in context.PersonChurches
                                 on p.PersonId equals c.PersonId
                               join g in groups
                                on p.PersonId equals g.LeaderId
                               where p.Email != null
                               && p.Email != string.Empty
                               && g.ChurchId == currentPerson.ChurchId
                               && c.ChurchId == currentPerson.ChurchId
                               select p)
                               .ToList();

                var administrators = (from p in context.People
                                      join c in context.PersonChurches
                                        on p.PersonId equals c.PersonId
                                      join g in groups
                                       on p.PersonId equals g.AdministratorId
                                      where p.Email != null
                                      && p.Email != string.Empty
                                      && g.ChurchId == currentPerson.ChurchId
                                      && c.ChurchId == currentPerson.ChurchId
                                      select p)
                                      .ToList();

                var people = new List<Person>();
                if (includeMembers)
                {
                    people = (from p in context.People
                              join c in context.PersonChurches
                                on p.PersonId equals c.PersonId
                              join pg in context.PersonGroups
                                on p.PersonId equals pg.PersonId
                              join g in groups
                            on pg.GroupId equals g.GroupId
                              where p.Email != null
                              && p.Email != string.Empty
                              && g.ChurchId == currentPerson.ChurchId
                              && c.ChurchId == currentPerson.ChurchId
                              select p).ToList();
                }

                foreach (Person p in leaders)
                {
                    if (p.HasValidEmail() && !addresses.Contains(p.Email))
                        addresses.Add(p.Email);
                }

                foreach (Person p in administrators)
                {
                    if (p.HasValidEmail() && !addresses.Contains(p.Email))
                        addresses.Add(p.Email);
                }

                foreach (Person p in people)
                {
                    if (p.HasValidEmail() && !addresses.Contains(p.Email))
                        addresses.Add(p.Email);
                }
            }

            return addresses;
        }

        public static List<string> FetchGroupCellPhoneNos(Person currentPerson, int groupId, List<int> selectedIds, bool selectedOnly)
        {
            var cellPhoneNos = new List<string>();
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var people = selectedOnly ? FetchPeopleInGroup(currentPerson, groupId, context, selectedIds).ToList() : FetchPeopleInGroup(currentPerson, groupId, context).ToList();

                cellPhoneNos.AddRange(from person in people where person.CellPhone != null && person.CellPhone.Trim() != string.Empty select person.CellPhone.Trim());
            }

            return cellPhoneNos;
        }

        public static List<string> FetchGroupLeaderCellPhoneNos(Person currentPerson, bool search, JqGridFilters filters, bool includeMembers)
        {
            var cellPhoneNos = new List<string>();
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var rules = filters == null ? null : filters.rules;
                var groups = FetchGroupList(currentPerson, search, rules, context);

                var leaders = (from g in groups
                               join p in context.People
                                on g.LeaderId equals p.PersonId
                               join po in context.PersonOptionalFields
                                on p.PersonId equals po.PersonId
                               join c in context.PersonChurches
                                 on p.PersonId equals c.PersonId
                               where po.OptionalFieldId == (int)OptionalFields.CellPhone
                               && po.Value != string.Empty
                               && g.ChurchId == currentPerson.ChurchId
                               && c.ChurchId == currentPerson.ChurchId
                               select po).ToList();

                var administrators = (from g in groups
                                      join p in context.People
                                       on g.AdministratorId equals p.PersonId
                                      join po in context.PersonOptionalFields
                                       on p.PersonId equals po.PersonId
                                      join c in context.PersonChurches
                                        on p.PersonId equals c.PersonId
                                      where po.OptionalFieldId == (int)OptionalFields.CellPhone
                                      && po.Value != string.Empty
                                      && g.ChurchId == currentPerson.ChurchId
                                      && c.ChurchId == currentPerson.ChurchId
                                      select po).ToList();

                var people = new List<PersonOptionalField>();
                if (includeMembers)
                {
                    people = (from g in groups
                              join pg in context.PersonGroups
                               on g.GroupId equals pg.PersonId
                              join p in context.People
                               on pg.PersonId equals p.PersonId
                              join po in context.PersonOptionalFields
                               on p.PersonId equals po.PersonId
                              join c in context.PersonChurches
                                on p.PersonId equals c.PersonId
                              where po.OptionalFieldId == (int)OptionalFields.CellPhone
                              && po.Value != string.Empty
                              && g.ChurchId == currentPerson.ChurchId
                              && c.ChurchId == currentPerson.ChurchId
                              select po).ToList();
                }

                foreach (var po in leaders)
                {
                    AddCellPhoneNoToList(cellPhoneNos, po);
                }

                foreach (var po in administrators)
                {
                    AddCellPhoneNoToList(cellPhoneNos, po);
                }

                foreach (var po in people)
                {
                    AddCellPhoneNoToList(cellPhoneNos, po);
                }
            }

            return cellPhoneNos;
        }

        public static List<string> FetchGroupAddresses(Person currentPerson, int groupId, List<int> selectedIds, bool selectedOnly)
        {
            List<string> addresses = new List<string>();
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                List<PersonViewModel> people = selectedOnly ? FetchPeopleInGroup(currentPerson, groupId, context, selectedIds).ToList() : FetchPeopleInGroup(currentPerson, groupId, context).ToList();

                foreach (PersonViewModel person in people)
                {
                    if (person.Email != null && person.Email != string.Empty)
                    {
                        addresses.Add(person.Email);
                    }
                }
            }

            return addresses;
        }

        public static IEnumerable<PersonViewModel> FetchPeopleInGroup(Person currentPerson, int groupId)
        {
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                return FetchPeopleInGroup(currentPerson, groupId, context).ToList();
            }
        }

        public static MapDataViewModel FetchPeopleInChurch(int churchId)
        {
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var mapDataModel = (from c in context.Churches
                                    where c.ChurchId == churchId
                                    select new MapDataViewModel
                                    {
                                        ChurchName = c.Name,
                                        ChurchLat = c.Address == null ? 0 : c.Address.Lat,
                                        ChurchLng = c.Address == null ? 0 : c.Address.Long
                                    }).FirstOrDefault();

                var members = (from f in context.Families
                               join p in context.People
                                on f.FamilyId equals p.FamilyId
                               join pc in context.PersonChurches
                                on p.PersonId equals pc.PersonId
                               where pc.ChurchId == churchId
                                && f.Address.Lat != 0
                                && f.Address.Long != 0
                               select new MapDataMember
                               {
                                   Surname = f.FamilyName,
                                   Lat = f.Address == null ? 0 : f.Address.Lat,
                                   Lng = f.Address == null ? 0 : f.Address.Long
                               }).ToList();
                mapDataModel.Members = members;

                var groups = (from g in context.Groups
                              join a in context.Addresses
                                on g.AddressId equals a.AddressId
                              where g.ChurchId == churchId
                               && a.Lat != 0
                               && a.Long != 0
                              select new MapDataHomeGroup
                              {
                                  GroupName = g.Name,
                                  Lat = a.Lat,
                                  Lng = a.Long
                              }).ToList();
                mapDataModel.HomeGroups = groups;

                return mapDataModel;
            }
        }

        public static IEnumerable<HomeGroupsViewModel> FetchGroupsPersonIsIn(int churchId, int personId)
        {
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var groups = (from g in context.PersonGroups
                              where g.Group.ChurchId == churchId
                                    && g.PersonId == personId
                              orderby g.Group.Name
                              select g.Group);

                return (from g in groups
                        select new HomeGroupsViewModel
                        {
                            GroupId = g.GroupId,
                            GroupName = g.Name,
                            LeaderName = g.Leader.Firstname + " " + g.Leader.Family.FamilyName,
                            LeaderId = g.LeaderId.HasValue ? g.LeaderId.Value : 0,
                            AdministratorName = g.Administrator == null ? string.Empty : g.Administrator.Firstname + " " + g.Administrator.Family.FamilyName,
                            AdministratorId = g.AdministratorId.HasValue ? g.AdministratorId.Value : 0,
                            AddressId = g.AddressId.HasValue ? g.AddressId.Value : 0,
                            Address1 = g.Address.Line1,
                            Address2 = g.Address.Line2,
                            Address3 = g.Address.Line3,
                            Address4 = g.Address.Line4,
                            AddressType = g.Address.AddressType,
                            Lat = g.Address != null ? g.Address.Lat : 0,
                            Lng = g.Address != null ? g.Address.Long : 0,
                            GroupClassificationId = g.GroupClassificationId.HasValue ? g.GroupClassificationId.Value : 0,
                            SuburbId = g.AddressId == null ? 0 : (g.Address.ChurchSuburbId == null ? 0 : (int)g.Address.ChurchSuburbId)
                        }).ToList();
            }
        }
        
        public static List<HomeGroupsViewModel> FetchHomeGroups(int churchId, Person person)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var groups = (from g in context.Groups.Include("Address").Include("GroupClassification")
                              where g.ChurchId == churchId
                              orderby g.Name
                              select g);

                if (person.HasPermission(Permissions.EditAllGroups))
                {
                    return (from g in groups
                            select new HomeGroupsViewModel
                            {
                                GroupId = g.GroupId,
                                GroupName = g.Name,
                                LeaderName = g.Leader.Firstname + " " + g.Leader.Family.FamilyName,
                                LeaderId = g.LeaderId.HasValue ? g.LeaderId.Value : 0,
                                AdministratorName = g.Administrator == null ? string.Empty : g.Administrator.Firstname + " " + g.Administrator.Family.FamilyName,
                                AdministratorId = g.AdministratorId.HasValue ? g.AdministratorId.Value : 0,
                                AddressId = g.AddressId.HasValue ? g.AddressId.Value : 0,
                                Address1 = g.Address.Line1,
                                Address2 = g.Address.Line2,
                                Address3 = g.Address.Line3,
                                Address4 = g.Address.Line4,
                                AddressType = g.Address.AddressType,
                                Lat = g.Address != null ? g.Address.Lat : 0,
                                Lng = g.Address != null ? g.Address.Long : 0,
                                GroupClassificationId = g.GroupClassificationId.HasValue ? g.GroupClassificationId.Value : 0,
                                SuburbId = g.AddressId == null ? 0 : (g.Address.ChurchSuburbId == null ? 0 : (int)g.Address.ChurchSuburbId)
                            }).ToList();
                }

                if (person.HasPermission(Permissions.EditOwnGroups))
                {
                    return (from g in groups
                            where g.LeaderId == person.PersonId || g.AdministratorId == person.PersonId
                            select new HomeGroupsViewModel
                            {
                                GroupId = g.GroupId,
                                GroupName = g.Name,
                                LeaderName = g.Leader.Firstname + " " + g.Leader.Family.FamilyName,
                                LeaderId = g.LeaderId.HasValue ? g.LeaderId.Value : 0,
                                AdministratorName = g.Administrator == null ? string.Empty : g.Administrator.Firstname + " " + g.Administrator.Family.FamilyName,
                                AdministratorId = g.AdministratorId.HasValue ? g.AdministratorId.Value : 0,
                                AddressId = g.AddressId.HasValue ? g.AddressId.Value : 0,
                                Address1 = g.Address.Line1,
                                Address2 = g.Address.Line2,
                                Address3 = g.Address.Line3,
                                Address4 = g.Address.Line4,
                                AddressType = g.Address.AddressType,
                                Lat = g.Address == null ? 0 : g.Address.Lat,
                                Lng = g.Address == null ? 0 : g.Address.Long
                            }).ToList();
                }

                return new List<HomeGroupsViewModel>();
            }
        }

        public static HomeGroupsViewModel FetchGroupInfo(Person currentPerson, int groupId)
        {
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var hg = (from g in context.Groups
                            where g.GroupId == groupId
                            select g).FirstOrDefault();

                var overseeingElder = hg.PersonLinkedToGroups.FirstOrDefault(p => p.Description == CacheNames.OverseeingElder);

                return new HomeGroupsViewModel()
                {
                    Address1 = hg.AddressId == null ? string.Empty : hg.Address.Line1,
                    Address2 = hg.AddressId == null ? string.Empty : hg.Address.Line2,
                    Address3 = hg.AddressId == null ? string.Empty : hg.Address.Line3,
                    Address4 = hg.AddressId == null ? string.Empty : hg.Address.Line4,
                    AddressId = hg.AddressId == null ? 0 : hg.AddressId.Value,
                    AddressType = hg.AddressId == null ? string.Empty : hg.Address.AddressType,
                    AdministratorId = hg.AdministratorId == null ? 0 : hg.AdministratorId.Value,
                    AdministratorName = hg.AdministratorId == null ? string.Empty : hg.Administrator.Firstname + " " + hg.Administrator.Family.FamilyName,
                    OverseeingElderId = overseeingElder == null ? 0 : overseeingElder.PersonId,
                    OverseeingElderName = overseeingElder == null ? string.Empty : overseeingElder.Person.Firstname + " " + overseeingElder.Person.Family.FamilyName,
                    ChurchName = hg.Church.Name,
                    GroupClassificationId = hg.GroupClassificationId.HasValue ? hg.GroupClassificationId.Value : 0,
                    GroupId = hg.GroupId,
                    GroupName = hg.Name,
                    Lat = hg.AddressId == null ? 0 : hg.Address.Lat,
                    LeaderId = hg.LeaderId == null ? 0 : hg.LeaderId.Value,
                    LeaderName = hg.LeaderId == null ? string.Empty : hg.Leader.Firstname + " " + hg.Leader.Family.FamilyName,
                    Lng = hg.AddressId == null ? 0 : hg.Address.Long,
                    SuburbId = hg.AddressId == null ? 0 : (hg.Address.ChurchSuburbId == null ? 0 : (int)hg.Address.ChurchSuburbId)
                };

            }
        }

        public static List<GroupDto> FetchGroupList(Person currentPerson)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                return context.Groups
                    .Where(g => g.ChurchId == currentPerson.ChurchId)
                    .Select(g => new GroupDto() { GroupId = g.GroupId, GroupName = g.Name })
                    .ToList();
            }
        }

        public static int SaveHomeGroup(Person currentPerson, HomeGroupsViewModel hgvm)
        {
            if (currentPerson.HasPermission(Permissions.EditGroups) || currentPerson.HasPermission(Permissions.AddGroups))
            {
                using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
                {
                    var hg = new Group();
                    if (hgvm.GroupId != 0)
                    {
                        hg = (from g in context.Groups
                              where g.GroupId == hgvm.GroupId
                              select g).FirstOrDefault();
                    }
                    else
                    {
                        hg.ChurchId = currentPerson.ChurchId;
                        hg.Created = DateTime.Now;
                        if (currentPerson.ChurchId == 3) //Ebenezer
                        {
                            hg.GroupTypeId = (int)GroupTypes.LifeGroup;
                        }
                        else
                        {
                            hg.GroupTypeId = (int)GroupTypes.HomeGroup;
                        }
                        context.Groups.AddObject(hg);
                    }

                    hg.Name = hgvm.GroupName;
                    if (hgvm.LeaderId == 0 || string.IsNullOrEmpty(hgvm.LeaderName))
                        hg.LeaderId = null;
                    else
                        hg.LeaderId = hgvm.LeaderId;
                    if (hgvm.AdministratorId == 0 || string.IsNullOrEmpty(hgvm.AdministratorName))
                        hg.AdministratorId = null;
                    else
                        hg.AdministratorId = hgvm.AdministratorId;
                    
                    hg.Changed = DateTime.Now;


                    //Check to see if the address already exists
                    if (hgvm.AddressId>0 || hgvm.Address1 != null || hgvm.Address2 != null || hgvm.Address3 != null || hgvm.Address4 != null || hgvm.SuburbId!=0)
                    {
                        var address = new Address();

                        if (hgvm.AddressId > 0)
                        {
                            address = (from a in context.Addresses
                                       where a.AddressId == hgvm.AddressId
                                       select a).FirstOrDefault();

                            if (address == null) //Should never happen, but just to be sure
                            {
                                address = new Address {Created = DateTime.Now};
                                hgvm.AddressId = 0;
                            }
                        }
                        else
                        {
                            address.Created = DateTime.Now;
                        }

                        address.Line1 = hgvm.Address1 ?? "";
                        address.Line2 = hgvm.Address2 ?? "";
                        address.Line3 = hgvm.Address3 ?? "";
                        address.Line4 = hgvm.Address4 ?? "";
                        address.AddressType = hgvm.AddressType ?? "";
                        address.Lat = hgvm.Lat;
                        address.Long = hgvm.Lng;
                        address.ChurchSuburbId = hgvm.SuburbId != 0 ? hgvm.SuburbId : (int?)null;
                        address.Changed = DateTime.Now;

                        if (hgvm.AddressId == 0)
                        {
                            context.Addresses.AddObject(address);
                        }

                        hg.Address = address;
                    }

                    hg.GroupClassificationId = hgvm.GroupClassificationId == 0 ? (int?)null : hgvm.GroupClassificationId;

                    context.SaveChanges();
                    if (hgvm.OverseeingElderId == 0 || string.IsNullOrEmpty(hgvm.OverseeingElderName))
                    {
                        var linkedPersonToDelete = context.PersonLinkedToGroups.FirstOrDefault(p => p.GroupId == hgvm.GroupId && p.Description == CacheNames.OverseeingElder);
                        if(linkedPersonToDelete!=null)
                            context.PersonLinkedToGroups.DeleteObject(linkedPersonToDelete);
                    }
                    else
                    {
                        var linkedPersonToDelete = context.PersonLinkedToGroups.FirstOrDefault(p => p.GroupId == hgvm.GroupId && p.Description == CacheNames.OverseeingElder);
                        if (linkedPersonToDelete != null)
                        {
                            context.PersonLinkedToGroups.DeleteObject(linkedPersonToDelete);
                        }
                        addNewElder(context, hg.GroupId, hgvm.OverseeingElderId);
                    }
                    context.SaveChanges();
                    return hg.GroupId;
                }
            }
            
            throw new ApplicationException("You do not have the required permission");
        }

        private static void addNewElder(oikonomosEntities context, int groupId, int overseeingElderId)
        {
            var newOverseeingElder = new PersonLinkedToGroup
            {
                GroupId = groupId,
                PersonId = overseeingElderId,
                Description = CacheNames.OverseeingElder
            };
            context.PersonLinkedToGroups.AddObject(newOverseeingElder);
        }

        public static void SaveGroupSettings(Person currentPerson, GroupDto groupSettings)
        {
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                //Check to see if the address already exists

                var address = new Address();

                if (groupSettings.AddressId > 0)
                {
                    address = (from a in context.Addresses
                               where a.AddressId == groupSettings.AddressId
                               select a).FirstOrDefault();

                    if (address == null) //Should never happen, but just to be sure
                    {
                        address = new Address {Created = DateTime.Now};
                        groupSettings.AddressId = 0;
                    }
                }
                else
                {
                    address.Created = DateTime.Now;
                }

                address.Line1 = groupSettings.Address1 ?? string.Empty;
                address.Line2 = groupSettings.Address2 ?? string.Empty;
                address.Line3 = groupSettings.Address3 ?? string.Empty;
                address.Line4 = groupSettings.Address4 ?? string.Empty;
                address.AddressType = groupSettings.AddressType ?? string.Empty;
                address.Lat = groupSettings.Lat;
                address.Long = groupSettings.Lng;
                address.Changed = DateTime.Now;

                if (groupSettings.AddressId == 0)
                {
                    context.Addresses.AddObject(address);
                }

                var group = (from g in context.Groups
                             where g.GroupId == groupSettings.GroupId
                             select g).FirstOrDefault();

                group.Address = address;

                context.SaveChanges();
            }
        }

        public static IEnumerable<string> FetchPeopleInARoleCellPhoneNos(Person currentPerson, int roleId)
        {
            var cellPhoneNos = new List<string>();
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var allPhoneNos = (from pc in context.PersonChurches
                                    join po in context.PersonOptionalFields
                                        on pc.PersonId equals po.PersonId
                                    where pc.ChurchId == currentPerson.ChurchId
                                          && pc.RoleId == roleId
                                          && po.OptionalFieldId == (int) OptionalFields.CellPhone
                                    select po
                                   ).ToList();
                
                foreach (var pn in allPhoneNos)
                {
                    AddCellPhoneNoToList(cellPhoneNos, pn);
                }
            }

            return cellPhoneNos;
        }


        public static IEnumerable<string> FetchPeopleInARoleEmails(Person currentPerson, int roleId)
        {
            var addresses = new List<string>();
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var peopleInRole = (from pc in context.PersonChurches
                                   join p in context.People
                                       on pc.PersonId equals p.PersonId
                                   where pc.ChurchId == currentPerson.ChurchId
                                         && pc.RoleId == roleId
                                   select p
                                   ).ToList();
                
                foreach (var p in peopleInRole)
                {
                    if (p.HasValidEmail() && !addresses.Contains(p.Email))
                        addresses.Add(p.Email);
                }
            }
            
            return addresses;
        }

        #region Private Methods

        private static void AddCellPhoneNoToList(ICollection<string> cellPhoneNos, PersonOptionalField po)
        {
            if (po.Value.Trim() == string.Empty)
                return;

            if (!cellPhoneNos.Contains(po.Value.Trim()))
                cellPhoneNos.Add(po.Value.Trim());
        }

        public static JqGridData FetchPeopleInGroupJQGrid(Person currentPerson, JqGridRequest request, int groupId)
        {
            using (var context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var people = FetchPeopleInGroup(currentPerson, groupId, context);

                if (request._search)
                {
                    foreach (var rule in request.filters.rules)
                    {
                        var ruleData = rule.data.ToLower();
                        //If we use rule.data throughout we get some strange errors in the SQL that Linq generates
                        switch (rule.field)
                        {
                            case "Firstname":
                                {
                                    people = (from p in people
                                              where p.Firstname.ToLower().Contains(ruleData)
                                              select p);
                                    break;
                                }
                            case "Surname":
                                {
                                    people = (from p in people
                                              where p.Surname.ToLower().Contains(ruleData)
                                              select p);
                                    break;
                                }
                        }
                    }
                }

                var totalRecords = people.Count();
                var sort = "Surname";
                var sortList = request.sidx.Split(',');
                if (sortList.Count() > 1)
                    sort = sortList[1].Trim();
                switch (sort)
                {
                    case "Firstname":
                    {
                        people = request.sord.ToLower() == "asc" ? people.OrderBy(p => p.RoleName).ThenBy(p => p.Firstname) : people.OrderBy(p => p.RoleName).ThenByDescending(p => p.Firstname);
                        break;
                    }
                    case "Surname":
                    {
                        people = request.sord.ToLower() == "asc" ? people.OrderBy(p => p.RoleName).ThenBy(p => p.Surname) : people.OrderBy(p => p.RoleName).ThenByDescending(p => p.Surname);
                        break;
                    }
                }

                if (request.rows > 0)
                    people = people.Skip((request.page - 1)*request.rows).Take(request.rows);
                else
                    request.page = 1;

                var peopleGridData = new JqGridData()
                {
                    total = request.rows>0 ? (int)Math.Ceiling((float)totalRecords / (float)request.rows) : 1,
                    page = request.page,
                    records = totalRecords,
                    rows = (from p in people.AsEnumerable()
                            select new JqGridRow()
                            {
                                id = p.PersonId.ToString(),
                                cell = new string[] {
                                                        p.PersonId.ToString(),
                                                        p.Firstname,
                                                        p.Surname,
                                                        p.HomePhone,
                                                        p.CellPhone,
                                                        p.Email,
                                                        p.RoleName
                                    }
                            }).ToArray()
                };

                return peopleGridData;
            }
        }

        private static IEnumerable<PersonViewModel> FetchPeopleInGroup(Person currentPerson, int groupId, oikonomosEntities context)
        {
            return FetchPeopleInGroup(currentPerson, groupId, context, null);
        }

        private static IEnumerable<PersonViewModel> FetchPeopleInGroup(Person currentPerson, int groupId, oikonomosEntities context, List<int> selectedIds)
        {
            var rolesToInclude = context
                                 .PermissionRoles
                                 .Where(p => p.PermissionId == (int)Permissions.IncludeInGroupList && p.Role.ChurchId == currentPerson.ChurchId)
                                 .Select(p => p.RoleId)
                                 .ToList();
            
            var people = (from p in context.People.Include("PersonOptionalField")
                          join pc in context.PersonChurches
                              on p.PersonId equals pc.PersonId 
                          join pg in context.PersonGroups
                              on p.PersonId equals pg.PersonId
                          join g in context.Groups
                              on pg.GroupId equals g.GroupId
                          join permissions in context.PermissionRoles
                              on pc.RoleId equals permissions.RoleId
                          orderby p.Family.FamilyName, p.PersonId
                          where pg.GroupId == groupId
                              && (permissions.PermissionId == (int)Permissions.IncludeInGroupList)
                              && pc.ChurchId == currentPerson.ChurchId
                              && rolesToInclude.Contains(pc.RoleId)
                          select new PersonViewModel
                          {
                              PersonId             = p.PersonId,
                              FamilyId             = p.Family.FamilyId,
                              Firstname            = p.Firstname,
                              Surname              = p.Family.FamilyName,
                              Email                = p.Email,
                              DateOfBirth_Value    = p.DateOfBirth,
                              AdministratorChecked = p.PersonId == g.AdministratorId ? "CHECKED" : "",
                              LeaderChecked        = p.PersonId == g.LeaderId ? "CHECKED" : "",
                              HomePhone            = p.Family.HomePhone,
                              CellPhone            = p.PersonOptionalFields.FirstOrDefault(po => po.OptionalFieldId == (int)OptionalFields.CellPhone).Value,
                              RoleName             = pc.Role.DisplayName
                          });

            if (selectedIds != null)
            {
                people = (from p in people
                          where selectedIds.Contains(p.PersonId)
                          select p);
            }

            return people.OrderBy(p => p.RoleName).ThenBy(p => p.Surname).ThenBy(p => p.PersonId);
        }

        private static IQueryable<Group> FetchGroupList(Person currentPerson, bool search, IEnumerable<JqGridFilterRule> rules, oikonomosEntities context)
        {
            IQueryable<Group> groups = null;
            if (currentPerson.HasPermission(Permissions.EditAllGroups))
            {
                groups = (from g in context.Groups.Include("Address").Include("GroupClassification")
                          where g.ChurchId == currentPerson.ChurchId
                          select g);
            }
            else if (!(currentPerson.HasPermission(Permissions.EditAllGroups)) && currentPerson.HasPermission(Permissions.EditOwnGroups))
            {
                groups = (from g in context.Groups
                          where (g.LeaderId == currentPerson.PersonId || g.AdministratorId == currentPerson.PersonId)
                          && g.ChurchId == currentPerson.ChurchId
                          select g);
            }

            if (groups == null)
                throw new Exception("Invalid security Role");

            if (search)
            {
                foreach (var rule in rules)
                {
                    //If we use rule.data throughout we get some strange errors in the SQL that Linq generates
                    switch (rule.field)
                    {
                        case "GroupName":
                        {
                            var groupName = rule.data;
                            groups = (from g in groups
                                where g.Name.Contains(groupName)
                                select g);
                            break;
                        }
                        case "LeaderName":
                        {
                            groups = Filters.ApplyNameSearchGroupLeader(rule.data, groups);
                            break;
                        }
                        case "Administrator":
                        {
                            groups = Filters.ApplyNameSearchGroupAdministrator(rule.data, groups);
                            break;
                        }
                        case "Suburb":
                        {
                            var suburb = rule.data;
                            groups = (from g in groups
                                where g.Address.ChurchSuburb.Suburb.Contains(suburb)
                                      || g.Address.Line3.Contains(suburb)
                                select g);
                            break;
                        }
                        case "GroupClassification":
                        {
                            var classification = rule.data;
                            groups = (from g in groups
                                where g.GroupClassification.Name.Contains(classification)
                                select g);
                            break;
                        }
                        case "OverseeingElder":
                        {
                            groups = Filters.ApplyNameSearchOverseeingElder(rule.data, groups);
                            break;
                        }
                    }
                }
            }

            return groups;
        }

        #endregion Private Methods

    }
}