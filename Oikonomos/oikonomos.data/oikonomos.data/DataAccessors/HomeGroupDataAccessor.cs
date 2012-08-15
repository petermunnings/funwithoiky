using System;
using System.Collections.Generic;
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

        public static JqGridData FetchHomeGroupsJQGrid(Person currentPerson, JqGridRequest request)
        {
            if (!(currentPerson.HasPermission(Permissions.EditOwnGroups) || currentPerson.HasPermission(Permissions.EditAllGroups)))
            {
                throw new Exception("Invalid security Role");
            }

            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                List<JqGridFilterRule> rules = request.filters == null ? null : request.filters.rules;
                var groups = FetchGroupList(currentPerson, request._search, rules, context);

                int totalRecords = groups.Count();

                switch (request.sidx)
                {
                    case "GroupName":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                groups = groups.OrderBy(g => g.Name).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                groups = groups.OrderByDescending(g => g.Name).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "LeaderName":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                groups = groups.OrderBy(g => g.Leader.Firstname).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                groups = groups.OrderByDescending(g => g.Leader.Firstname).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "Administrator":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                groups = groups.OrderBy(g => g.Administrator.Firstname).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                groups = groups.OrderByDescending(g => g.Administrator.Firstname).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "Suburb":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                groups = groups.OrderBy(g => g.Address.ChurchSuburb.Suburb).ThenBy(g => g.Address.Line3).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                groups = groups.OrderByDescending(g => g.Address.ChurchSuburb.Suburb).ThenByDescending(g => g.Address.Line3).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "GroupClassification":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                groups = groups.OrderBy(g => g.GroupClassification.Name).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                groups = groups.OrderByDescending(g => g.GroupClassification.Name).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                }

                JqGridData sitesGridData = new JqGridData()
                {
                    total = (int)Math.Ceiling((float)totalRecords / (float)request.rows),
                    page = request.page,
                    records = totalRecords,
                    rows = (from g in groups.AsEnumerable()
                            select new JqGridRow()
                            {
                                id = g.GroupId.ToString(),
                                cell = new string[] {
                                                    g.GroupId.ToString(),
                                                    g.Name,
                                                    g.Leader==null? string.Empty : g.Leader.Firstname + " " + g.Leader.Family.FamilyName,
                                                    g.Administrator==null? string.Empty : g.Administrator.Firstname + " " + g.Administrator.Family.FamilyName,
                                                    g.Address==null? string.Empty : g.Address.ChurchSuburb==null?g.Address.Line3:g.Address.ChurchSuburb.Suburb,
                                                    g.GroupClassification==null? string.Empty :g.GroupClassification.Name
                                }
                            }).ToArray()
                };


                return sitesGridData;
            }
        }

        public static JqGridData FetchPeopleNotInAHomeGroupJQGrid(Person currentPerson, JqGridRequest request)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
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
                                         && includedRoles.Contains(p.RoleId)
                                         select p);

                int totalRecords = peopleNotInGroups.Count();

                switch (request.sidx)
                {
                    case "Firstname":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                peopleNotInGroups = peopleNotInGroups.OrderBy(p => p.Firstname).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                peopleNotInGroups = peopleNotInGroups.OrderByDescending(p => p.Firstname).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "Surname":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                peopleNotInGroups = peopleNotInGroups.OrderBy(p => p.Family.FamilyName).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                peopleNotInGroups = peopleNotInGroups.OrderByDescending(p => p.Family.FamilyName).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                }

                JqGridData sitesGridData = new JqGridData()
                {
                    total = (int)Math.Ceiling((float)totalRecords / (float)request.rows),
                    page = request.page,
                    records = totalRecords,
                    rows = (from p in peopleNotInGroups.AsEnumerable()
                            select new JqGridRow()
                            {
                                id = p.PersonId.ToString(),
                                cell = new string[] {
                                                    p.PersonId.ToString(),
                                                    p.Firstname,
                                                    p.Family.FamilyName,
                                                    p.Family.HomePhone,
                                                    p.PersonOptionalFields.FirstOrDefault(c => c.OptionalFieldId == (int)OptionalFields.CellPhone)==null?"":p.PersonOptionalFields.Where<PersonOptionalField>(c => c.OptionalFieldId == (int)OptionalFields.CellPhone).FirstOrDefault().Value,
                                                    p.Email
                                }
                            }).ToArray()
                };
                return sitesGridData;
            }
        }


        public static List<PersonListViewModel> FetchPeopleNotInAGroup(Person currentPerson)
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
                        && includedRoles.Contains(p.RoleId)
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

        public static string FetchHomeGroupName(int groupId)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                return (from g in context.Groups
                        where g.GroupId == groupId
                        select g.Name).FirstOrDefault();
            }
        }

        public static string FetchHomeGroupName(Person currentPerson, out bool displayList, ref int? groupId)
        {
            displayList = false;
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var homeGroups = (from g in context.Groups
                                  where (g.LeaderId == currentPerson.PersonId || g.AdministratorId == currentPerson.PersonId)
                                  && g.ChurchId==currentPerson.ChurchId
                                  orderby g.Name
                                  select g);
                if (homeGroups.Count() == 0)
                {
                    displayList = false;
                    return string.Empty;
                }

                if (homeGroups.Count() == 1)
                {
                    displayList = false;
                }
                else
                {
                    displayList = true;
                }

                if(!groupId.HasValue)
                    groupId = homeGroups.First().GroupId;
                return homeGroups.First().Name;
            }
        }

        public static bool DeleteHomeGroup(int groupId, ref string message)
        {
            bool success = false;
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                //Check to see if there is anyone in the group
                var peopleInGroup = (from pg in context.PersonGroups
                                     where pg.GroupId == groupId
                                     select pg).Count();
                if (peopleInGroup == 0)
                {
                    //Delete group
                    var groupToDelete = (from g in context.Groups
                                         where g.GroupId == groupId
                                         select g).FirstOrDefault();
                    if (groupToDelete != null)
                    {
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
                    message = "Cannot delete group, there are still people in the group";
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

        public static void RemovePersonFromGroup(Person currentPerson, int groupId, int personId)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                if (currentPerson.HasPermission(Permissions.RemovePersonFromGroup))
                {
                    //Fetch the record
                    var personToRemove = (from pg in context.PersonGroups
                                          where pg.PersonId == personId
                                          && pg.GroupId == groupId
                                          select pg).FirstOrDefault();
                    if (personToRemove != null)
                    {
                        //Remove them
                        context.PersonGroups.DeleteObject(personToRemove);
                        context.SaveChanges();
                    }
                }
            }
        }

        public static void AddPersonToGroup(int groupId, int personId)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                //First check to see if they are not already there
                var check = (from pg in context.PersonGroups
                             where pg.PersonId == personId
                             && pg.GroupId == groupId
                             select pg).FirstOrDefault();
                if (check == null)
                {
                    //Add them
                    PersonGroup personGroup = new PersonGroup();
                    context.PersonGroups.AddObject(personGroup);
                    personGroup.Created = DateTime.Now;
                    personGroup.Changed = DateTime.Now;
                    personGroup.PersonId = personId;
                    personGroup.GroupId = groupId;
                    personGroup.Joined = DateTime.Now;

                    context.SaveChanges();
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
            List<string> cellPhoneNos = new List<string>();
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                List<PersonViewModel> people = selectedOnly ? FetchPeopleInGroup(currentPerson, groupId, context, selectedIds).ToList() : FetchPeopleInGroup(currentPerson, groupId, context).ToList();

                foreach (PersonViewModel person in people)
                {
                    if (person.CellPhone != null && person.CellPhone.Trim() != string.Empty)
                    {
                        cellPhoneNos.Add(Utils.ConvertCellPhoneToInternational(person.CellPhone, currentPerson.Church.Country));
                    }
                }
            }

            return cellPhoneNos;
        }

        public static List<string> FetchGroupLeaderCellPhoneNos(Person currentPerson, bool search, JqGridFilters filters, bool includeMembers)
        {
            List<string> cellPhoneNos = new List<string>();
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                List<JqGridFilterRule> rules = filters == null ? null : filters.rules;
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

                foreach (PersonOptionalField po in leaders)
                {
                    AddCellPhoneNoToList(currentPerson, cellPhoneNos, po);
                }

                foreach (PersonOptionalField po in administrators)
                {
                    AddCellPhoneNoToList(currentPerson, cellPhoneNos, po);
                }

                foreach (PersonOptionalField po in people)
                {
                    AddCellPhoneNoToList(currentPerson, cellPhoneNos, po);
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
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                return FetchPeopleInGroup(currentPerson, groupId, context).ToList();
            }
        }

        public static MapDataViewModel FetchPeopleInChurch(int churchId)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
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
                               where f.ChurchId == churchId
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
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                Group hg = (from g in context.Groups
                            where g.GroupId == groupId
                            select g).FirstOrDefault();

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

        public static void SaveHomeGroup(Person currentPerson, HomeGroupsViewModel hgvm)
        {
            if (currentPerson.HasPermission(Permissions.EditGroups) || currentPerson.HasPermission(Permissions.AddGroups))
            {
                using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
                {
                    Group hg = new Group();
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
                    if (hgvm.LeaderId == 0 || hgvm.LeaderName == null || hgvm.LeaderName == string.Empty)
                        hg.LeaderId = null;
                    else
                        hg.LeaderId = hgvm.LeaderId;
                    if (hgvm.AdministratorId == 0 || hgvm.AdministratorName == null || hgvm.AdministratorName == string.Empty)
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
                                address = new Address();
                                address.Created = DateTime.Now;
                                hgvm.AddressId = 0;
                            }
                        }
                        else
                        {
                            address.Created = DateTime.Now;
                        }

                        address.Line1 = hgvm.Address1 == null ? "" : hgvm.Address1;
                        address.Line2 = hgvm.Address2 == null ? "" : hgvm.Address2;
                        address.Line3 = hgvm.Address3 == null ? "" : hgvm.Address3;
                        address.Line4 = hgvm.Address4 == null ? "" : hgvm.Address4;
                        address.AddressType = hgvm.AddressType == null ? "" : hgvm.AddressType;
                        address.Lat = hgvm.Lat == null ? 0 : hgvm.Lat;
                        address.Long = hgvm.Lng == null ? 0 : hgvm.Lng;
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
                }
            }
            else
            {
                throw new ApplicationException("You do not have the required permission");
            }
        }

        public static void SaveGroupSettings(Person currentPerson, GroupDto groupSettings)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
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
                        address = new Address();
                        address.Created = DateTime.Now;
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

        #region Private Methods

        private static void AddCellPhoneNoToList(Person currentPerson, List<string> cellPhoneNos, PersonOptionalField po)
        {
            if (po.Value.Trim() == string.Empty)
                return;

            string internationalCellPhoneNo = Utils.ConvertCellPhoneToInternational(po.Value, currentPerson.Church.Country);
            if (!cellPhoneNos.Contains(internationalCellPhoneNo))
                cellPhoneNos.Add(internationalCellPhoneNo);
        }

        public static JqGridData FetchPeopleInGroupJQGrid(Person currentPerson, JqGridRequest request, int groupId)
        {
            using (oikonomosEntities context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString))
            {
                var people = FetchPeopleInGroup(currentPerson, groupId, context);

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
                                              where p.Surname.Contains(ruleData)
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
                                people = people.OrderBy(p => p.Surname).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                people = people.OrderByDescending(p => p.Surname).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            break;
                        }
                    case "Role":
                        {
                            if (request.sord.ToLower() == "asc")
                            {
                                people = people.OrderBy(p => p.RoleName).Skip((request.page - 1) * request.rows).Take(request.rows);
                            }
                            else
                            {
                                people = people.OrderByDescending(p => p.RoleName).Skip((request.page - 1) * request.rows).Take(request.rows);
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
                                 .Where(p => p.PermissionId == (int)Permissions.IncludeInChurchList && p.Role.ChurchId == currentPerson.ChurchId)
                                 .Select(p => p.RoleId)
                                 .ToList();
            
            var people = (from p in context.People.Include("PersonOptionalField")
                          join pg in context.PersonGroups
                              on p.PersonId equals pg.PersonId
                          join g in context.Groups
                              on pg.GroupId equals g.GroupId
                          join permissions in context.PermissionRoles
                              on p.RoleId equals permissions.RoleId
                          orderby p.Family.FamilyName, p.PersonId
                          where pg.GroupId == groupId
                              && (permissions.PermissionId == (int)Permissions.Login)
                              && rolesToInclude.Contains(p.RoleId)
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
                              RoleName             = p.Role.DisplayName
                          });

            if (selectedIds != null)
            {
                people = (from p in people
                          where selectedIds.Contains(p.PersonId)
                          select p);
            }

            return people.OrderBy(p => p.RoleName).ThenBy(p => p.Surname).ThenBy(p => p.PersonId);
        }

        private static IQueryable<Group> FetchGroupList(Person currentPerson, bool search, List<JqGridFilterRule> rules, oikonomosEntities context)
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
                foreach (JqGridFilterRule rule in rules)
                {
                    //If we use rule.data throughout we get some strange errors in the SQL that Linq generates
                    switch (rule.field)
                    {
                        case "GroupName":
                            {
                                string groupName = rule.data;
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
                                string suburb = rule.data;
                                groups = (from g in groups
                                          where g.Address.ChurchSuburb.Suburb.Contains(suburb)
                                          || g.Address.Line3.Contains(suburb)
                                          select g);
                                break;
                            }
                        case "GroupClassification":
                            {
                                string classification = rule.data;
                                groups = (from g in groups
                                          where g.GroupClassification.Name.Contains(classification)
                                          select g);
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