using System.Collections.Generic;
using System.Web.Mvc;
using oikonomos.data.DataAccessors;
using oikonomos.data;
using oikonomos.common.Models;
using Lib.Web.Mvc.JQuery.JqGrid;
using System.Threading.Tasks;
using Facebook;
using oikonomos.common;
using System;
using oikonomos.data.Services;

namespace oikonomos.web.Controllers
{
    public class AjaxController : Controller
    {
        public JsonResult InitializeChurchSettingsViewModel()
        {
            return Json(new ChurchSettingsViewModel { UITheme = "start", SystemName = "Oiky", AddressType = "street_address", BulkSmsUsername = string.Empty, BulkSmsPassword = string.Empty }, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult PersonAutoComplete(string term)
        {
            AutoCompleteViewModel[] data = new AutoCompleteViewModel[0];
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (Request.UrlReferrer.PathAndQuery == "/Home/Groups")
                {
                    data = PersonDataAccessor.FetchPersonAutoComplete(term, currentPerson, true);
                }
                else
                {
                    data = PersonDataAccessor.FetchPersonAutoComplete(term, currentPerson, false);
                }
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FamilyAutoComplete(string term)
        {
            AutoCompleteViewModel[] data = new AutoCompleteViewModel[0];
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                data = PersonDataAccessor.FetchFamilyAutoComplete(term, currentPerson.ChurchId);
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ResetPassword(string emailAddress)
        {
            string message = string.Empty;

            try
            {
                if (Utils.ValidEmailAddress(emailAddress))
                {
                    message = PersonDataAccessor.ResetPassword(emailAddress);
                }
                else
                {
                    message = "Invalid email address";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            var response = new { Message = message };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult SavePersonEvents(PersonEventViewModel personEvents)
        {
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                EventDataAccessor.SavePersonEvents(personEvents, (Person)Session[SessionVariable.LoggedOnPerson]);
            }

            var response = new { SessionTimeOut = sessionTimedOut };
            return Json(response, JsonRequestBehavior.DenyGet);
        }
        
        public JsonResult SavePerson(PersonViewModel person)
        {
            bool sessionTimedOut = false;
            int personId = 0;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                personId = PersonDataAccessor.SavePerson(person, currentPerson);
            }

            var response = new { PersonId = personId, SessionTimeOut = sessionTimedOut };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult DeletePerson(int personId)
        {
            bool sessionTimedOut = false;
            string message=string.Empty;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.PersonId == personId)
                {
                    message = "You cannot delete yourself";
                }
                else
                {
                    //TODO Check for User Roles
                    PersonDataAccessor.DeletePerson(personId, currentPerson);
                }
            }

            var response = new { SessionTimeOut = sessionTimedOut,
                                    Message = message};
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult SendEmailAndPassword(int personId)
        {
            bool sessionTimedOut = false;
            bool emailSent = false;
            string message = string.Empty;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                emailSent=PersonDataAccessor.SendEmailAndPassword(currentPerson, personId, ref message);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message,
                EmailSent = emailSent
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchChurchList(JqGridRequest request)
        {
            JqGridData jqGridData = new JqGridData();
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                jqGridData = PersonDataAccessor.FetchChurchListJQGrid(currentPerson, request);
            }

            return Json(jqGridData); 
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchEventListForPerson(JqGridRequest request, int personId)
        {
            JqGridData jqGridData = new JqGridData();
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                jqGridData = EventDataAccessor.FetchEventListJQGrid(currentPerson, personId, request);
            }

            return Json(jqGridData);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchGroupsForPerson(JqGridRequest request, int personId)
        {
            JqGridData jqGridData = new JqGridData();
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                jqGridData = PersonDataAccessor.FetchGroupsForPersonJQGrid(currentPerson, personId, request);
            }

            return Json(jqGridData);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchEventList(JqGridRequest request, DateTime fromDate, DateTime toDate)
        {
            JqGridData jqGridData = new JqGridData();
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                jqGridData = EventDataAccessor.FetchEventListJQGrid(currentPerson, fromDate, toDate, request);
            }

            return Json(jqGridData);
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchPeople(JqGridRequest request, int roleId)
        {
            JqGridData jqGridData = new JqGridData();
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                jqGridData = PersonDataAccessor.FetchPeopleJQGrid(currentPerson, request, roleId);
            }

            return Json(jqGridData);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchPeopleInGroupForAttendance(int groupId)
        {
            List<PersonViewModel> people = new List<PersonViewModel>();
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                people = (List<PersonViewModel>)GroupDataAccessor.FetchPeopleInGroup(currentPerson, groupId);
            }

            return Json(new { People = people });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchPeopleInGroup(JqGridRequest request, int groupId)
        {
            JqGridData jqGridData = new JqGridData();
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                jqGridData = GroupDataAccessor.FetchPeopleInGroupJQGrid(currentPerson, request, groupId);
            }

            return Json(jqGridData);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchSites(JqGridRequest request)
        {
            JqGridData jqGridData = new JqGridData();
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                jqGridData = ChurchDataAccessor.FetchSitesJQGrid(currentPerson, request);
            }

            return Json(jqGridData);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchPermissionsLinked(JqGridRequest request, int roleId)
        {
            JqGridData jqGridData = new JqGridData();
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                jqGridData = PermissionDataAccessor.FetchPermissionsForRoleJQGrid(currentPerson, request, roleId);
            }

            return Json(jqGridData);
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchPermissionsUnLinked(JqGridRequest request, int roleId)
        {
            JqGridData jqGridData = new JqGridData();
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                jqGridData = PermissionDataAccessor.FetchPermissionsNotInRoleJQGrid(currentPerson, request, roleId);
            }

            return Json(jqGridData);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult ChangeChurchTo(int churchId)
        {
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                SelectNewChurch(churchId, currentPerson);
            }

            return Json(new { message = "Success" });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult SelectNewChurch(int churchId)
        {
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(Permissions.Login))
                {
                    SelectNewChurch(churchId, currentPerson);
                }
            }

            return Json(new { message = "Success" });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchGroupList(JqGridRequest request)
        {
            try
            {
                JqGridData jqGridData = new JqGridData();
                if (Session[SessionVariable.LoggedOnPerson] != null)
                {
                    Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                    if (currentPerson.HasPermission(common.Permissions.EditAllGroups) || currentPerson.HasPermission(Permissions.EditOwnGroups))
                    {
                        jqGridData = GroupDataAccessor.FetchHomeGroupsJQGrid(currentPerson, request);
                    }
                }

                return Json(jqGridData);
            }
            catch (Exception ex)
            {
                try
                {
                    Email.SendExceptionEmail(ex);
                }
                catch { }
                return Json(null);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchHomeGroupList(JqGridRequest request)
        {
            try
            {
                JqGridData jqGridData = new JqGridData();
                var message = string.Empty;
                if (Session[SessionVariable.LoggedOnPerson] != null)
                {
                    var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                    message = String.Format("Current Person:{0}", currentPerson.Fullname);
                    
                }

                message += " is trying to access FetchHomeGroupList";

                Email.SendSystemEmail("Javascript file not updated", message);

                return Json(jqGridData);
            }
            catch (Exception ex)
            {
                try
                {
                    Email.SendExceptionEmail(ex);
                }
                catch { }
                return Json(null);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchPeopleNotInHomeGroup(JqGridRequest request)
        {
            JqGridData jqGridData = new JqGridData();
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(common.Permissions.ViewPeopleNotInAnyGroup))
                {
                    jqGridData = GroupDataAccessor.FetchPeopleNotInAHomeGroupJQGrid(currentPerson, request);
                }
            }

            return Json(jqGridData);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult AddPermissionsToRole(int roleId, List<int> permissionIds)
        {
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                PermissionDataAccessor.AddPermissionsToRole(currentPerson, roleId, permissionIds);
            }

            return Json(new { Message = "success" });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult RemovePermissionsFromRole(int roleId, List<int> permissionIds)
        {
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                PermissionDataAccessor.RemovePermissionsFromRole(currentPerson, roleId, permissionIds);
            }

            return Json(new { Message = "success" });
        }

        public JsonResult FetchSite(int siteId)
        {
            SiteSettingsViewModel site = new SiteSettingsViewModel();
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                site = ChurchDataAccessor.FetchSite(currentPerson, siteId);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Site = site
            };
            return Json(response); 
        }

        public JsonResult DeleteSite(int siteId)
        {
            bool sessionTimedOut = false;
            string message=string.Empty;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                message = ChurchDataAccessor.DeleteSite(currentPerson, siteId);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message
            };
            return Json(response);
        }

        public JsonResult FetchOptionalFields()
        {
            List<OptionalFieldViewModel> optionalFields = new List<OptionalFieldViewModel>();
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                optionalFields = SettingsDataAccessor.FetchChurchOptionalFields(currentPerson.ChurchId);
            }

            var response = new { SessionTimeOut = sessionTimedOut,
                                 OptionalFields = optionalFields};
            return Json(response, JsonRequestBehavior.AllowGet); 
        }

        public JsonResult FetchGroupClassifications()
        {
            List<GroupClassificationViewModel> groupClassifications = new List<GroupClassificationViewModel>();
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                groupClassifications = SettingsDataAccessor.FetchGroupClassifications(currentPerson);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                GroupClassifications = groupClassifications
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddGroupClassification(string groupClassification)
        {
            List<GroupClassificationViewModel> groupClassifications = new List<GroupClassificationViewModel>();
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                groupClassifications = SettingsDataAccessor.AddGroupClassification(currentPerson, groupClassification);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                GroupClassifications = groupClassifications
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteGroupClassification(int groupClassificationId)
        {
            List<GroupClassificationViewModel> groupClassifications = new List<GroupClassificationViewModel>();
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                groupClassifications = SettingsDataAccessor.DeleteGroupClassification(currentPerson, groupClassificationId);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                GroupClassifications = groupClassifications
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FetchSuburbs()
        {
            List<SuburbViewModel> suburbs = new List<SuburbViewModel>();
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                suburbs = SettingsDataAccessor.FetchSuburbs(currentPerson);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Suburbs = suburbs
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddSuburb(string suburb)
        {
            List<SuburbViewModel> suburbs = new List<SuburbViewModel>();
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                suburbs = SettingsDataAccessor.AddSuburb(currentPerson, suburb);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Suburbs = suburbs
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult DeleteSuburb(int suburbId)
        {
            List<SuburbViewModel> suburbs = new List<SuburbViewModel>();
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                suburbs = SettingsDataAccessor.DeleteSuburb(currentPerson, suburbId);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Suburbs = suburbs
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FetchEventTypes(string eventFor)
        {
            List<EventTypeViewModel> eventTypes = new List<EventTypeViewModel>();
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                eventTypes = SettingsDataAccessor.FetchEventTypes(currentPerson, eventFor);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                EventTypes = eventTypes
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddEventType(string eventType, string eventFor)
        {
            try
            {
                List<EventTypeViewModel> eventTypes = new List<EventTypeViewModel>();
                bool sessionTimedOut = false;
                if (Session[SessionVariable.LoggedOnPerson] == null)
                {
                    sessionTimedOut = true;
                }
                else
                {
                    Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                    eventTypes = SettingsDataAccessor.AddEventType(currentPerson, eventType, eventFor);
                }

                var response = new
                {
                    SessionTimeOut = sessionTimedOut,
                    EventTypes = eventTypes
                };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Email.SendExceptionEmail(ex);
                List<EventTypeViewModel> eventTypes = new List<EventTypeViewModel>();
                EventTypeViewModel et = new EventTypeViewModel();
                et.EventTypeId = 0;
                et.EventType = "Error saving event";
                eventTypes.Add(et);
                var response = new
                {
                    SessionTimeOut = false,
                    EventTypes = eventTypes
                };
                return Json(response); 

            }
        }

        public JsonResult DeleteEventType(int eventTypeId, string eventFor)
        {
            List<EventTypeViewModel> eventTypes = new List<EventTypeViewModel>();
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                eventTypes = SettingsDataAccessor.DeleteEventType(currentPerson, eventTypeId, eventFor);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                EventTypes = eventTypes
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddPersonToFamily(int familyId, int personId)
        {
            List<FamilyMemberViewModel> familyMembers = new List<FamilyMemberViewModel>();
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                //TODO Check for User Roles
                if (personId > 0 && familyId > 0)
                {
                    familyMembers = PersonDataAccessor.AddPersonToFamily(familyId, personId);
                }
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                FamilyMembers = familyMembers
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }


        public JsonResult AddPersonToGroup(int groupId, int personId)
        {
            List<PersonViewModel> people = new List<PersonViewModel>();

            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                //TODO Check for User Roles
                if(personId>0 && groupId>0)
                {
                    GroupDataAccessor.AddPersonToGroup(groupId, personId);
                }
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult RemovePersonFromGroup(int groupId, int personId)
        {
            List<PersonViewModel> people = new List<PersonViewModel>();

            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                //TODO Check for User Roles
                if (personId > 0 && groupId > 0)
                {
                    GroupDataAccessor.RemovePersonFromGroup(currentPerson, groupId, personId);
                }
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public void SetHomeGroupLeader(int groupId, int leaderId)
        {
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                //TODO Check for User Roles
                if (leaderId > 0 && groupId > 0)
                {
                    GroupDataAccessor.SetHomeGroupLeader(currentPerson, groupId, leaderId);
                }
            }
        }

        public void SetHomeGroupAdministrator(int groupId, int administratorId)
        {
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                //TODO Check for User Roles
                if (administratorId > 0 && groupId > 0)
                {
                    GroupDataAccessor.SetHomeGroupAdministrator(currentPerson, groupId, administratorId);
                }
            }
        }

        public JsonResult DeleteHomeGroup(int groupId)
        {
            string message=string.Empty;
            bool success = false;
            List<HomeGroupsViewModel> homeGroups = new List<HomeGroupsViewModel>();
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(common.Permissions.DeleteGroups))
                {
                    success = GroupDataAccessor.DeleteHomeGroup(groupId, ref message);
                }
                else
                {
                    message = "You do not have permission to delete a homegroup";
                }
                
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message,
                Success = success
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }


        public JsonResult FetchPeopleInChurch()
        {
            MapDataViewModel mapData = new MapDataViewModel();
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                //TODO Check for User Roles
                mapData = GroupDataAccessor.FetchPeopleInChurch(currentPerson.ChurchId);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                MapData = mapData
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult FetchGroupAttendanceGridSetup()
        {
            GridSetupViewModel gridSetup = new GridSetupViewModel();
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(Permissions.ViewGroupAttendance))
                {
                    gridSetup = GroupDataAccessor.FetchGroupAttendanceGridSetup();
                }
            }

            return Json(gridSetup, JsonRequestBehavior.DenyGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchGroupAttendance(JqGridRequest request)
        {
            JqGridData jqGridData = new JqGridData();
            if (Session[SessionVariable.LoggedOnPerson] != null)
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(common.Permissions.ViewGroupAttendance))
                {
                    jqGridData = GroupDataAccessor.FetchGroupAttendanceJQGrid(currentPerson, request);
                }
            }

            return Json(jqGridData);
        }

        public JsonResult FetchAttendance(int groupId, DateTime date)
        {
            List<AttendanceEventViewModel> attendance = new List<AttendanceEventViewModel>();
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                attendance = EventDataAccessor.FetchGroupAttendance(currentPerson, groupId, date);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Attendance = attendance
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult SaveHomeGroupEvent(HomeGroupEventViewModel hgEvent)
        {
            string message = string.Empty;
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];

                EventDataAccessor.SaveHomeGroupEvent(currentPerson, hgEvent);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message
            };
            return Json(response, JsonRequestBehavior.DenyGet); 
        }


        public JsonResult FetchFamilyMembers(int personId, int familyId)
        {
            var response = new
            {
                FamilyMembers = PersonDataAccessor.FetchFamilyMembers(personId, familyId)
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult FetchPerson(int personId)
        {
            PersonViewModel personViewModel = new PersonViewModel();
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (personId > 0)
                {
                    personViewModel = PersonDataAccessor.FetchPersonViewModel(personId, currentPerson);

                    if (personViewModel != null && personViewModel.FacebookId == null && personId != currentPerson.PersonId)
                    {
                        if (Session["FacebookClient"] != null)
                        {
                            FacebookClient client = (FacebookClient)Session["FacebookClient"];
                            //Search for facebook Id
                            Task.Factory.StartNew(() => SearchForFacebookId(personId, personViewModel.Firstname, personViewModel.Surname, client));
                        }
                    }
                }
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Person = personViewModel
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult FetchGroupInfo(int groupId)
        {
            bool sessionTimedOut = false;
            var groupInfo = new HomeGroupsViewModel();
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                groupInfo = GroupDataAccessor.FetchGroupInfo(currentPerson, groupId);
            }

            var response = new
            {
                GroupInfo = groupInfo,
                SessionTimeOut = sessionTimedOut
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult SaveHomeGroup(HomeGroupsViewModel hgvm)
        {
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                //TODO Check for User Roles
                GroupDataAccessor.SaveHomeGroup(currentPerson, hgvm);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult SaveOptionalFields(List<OptionalFieldViewModel> optionalFields)
        {
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                //TODO Check for User Roles
                ChurchDataAccessor.SaveChurchOptionalFields(currentPerson.ChurchId, optionalFields);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                OptionalFields = optionalFields
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveGroupAddress(GroupDto groupSettings)
        {
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                //TODO Check for User Roles
                GroupDataAccessor.SaveGroupSettings(currentPerson, groupSettings);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = "Address Saved"
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveBulkSmsDetails(string BulkSmsUsername, string BulkSmsPassword)
        {
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                //TODO Check for User Roles
                SettingsDataAccessor.SaveBulkSmsDetails(currentPerson, BulkSmsUsername, BulkSmsPassword);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = "Bulk Sms Details Saved"
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveChurchContactDetails(ChurchSettingsViewModel churchSettings)
        {
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                //TODO Check for User Roles
                SettingsDataAccessor.SaveChurchContactDetails(currentPerson, churchSettings);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = "Church Contact Details Saved"
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateNewChurch(ChurchSettingsViewModel churchSettings)
        {
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                SettingsDataAccessor.CreateNewChurch(currentPerson, churchSettings);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = "Church Created"
            };

            return Json(response);
        }

        public JsonResult SaveSite(SiteSettingsViewModel siteSettings)
        {
            bool sessionTimedOut = false;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                ChurchDataAccessor.SaveSite(currentPerson, siteSettings);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = "Site Saved"
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        

        public JsonResult ChangePassword(string currentPassword, string newPassword)
        {
            bool sessionTimedOut = false;
            string message = string.Empty;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                message=PersonDataAccessor.ChangePassword(currentPerson.PersonId, currentPassword, newPassword);
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }


        public JsonResult FetchGroupLeaderEmails(bool search, JqGridFilters filters, bool includeMembers)
        {
            bool sessionTimedOut = false;
            string message = string.Empty;
            List<string> addresses = new List<string>();
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
                message = ExceptionMessage.SessionTimedOut;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(Permissions.EmailGroupLeaders))
                {
                    Session[SessionVariable.EmailAddresses] = GroupDataAccessor.FetchGroupLeaderAddresses(currentPerson, search, filters, includeMembers);
                }
                else
                {
                    message = ExceptionMessage.InvalidCredentials;
                }
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult FetchGroupLeaderCellPhoneNos(bool search, JqGridFilters filters, bool includeMembers)
        {
            bool sessionTimedOut = false;
            string message = string.Empty;
            int noNos = 0;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
                message = ExceptionMessage.SessionTimedOut;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(Permissions.SmsGroupLeaders))
                {
                    Session[SessionVariable.CellPhoneNos] = GroupDataAccessor.FetchGroupLeaderCellPhoneNos(currentPerson, search, filters, includeMembers);
                    noNos = ((List<string>)Session[SessionVariable.CellPhoneNos]).Count;
                }
                else
                {
                    message = ExceptionMessage.InvalidCredentials;
                }
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message,
                NoNos = noNos
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult FetchGroupCellPhoneNos(int groupId, List<int> selectedIds, bool selectedOnly)
        {
            bool sessionTimedOut = false;
            string message = string.Empty;
            int noNos = 0;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
                message = ExceptionMessage.SessionTimedOut;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(Permissions.SmsGroupMembers) || currentPerson.HasPermission(Permissions.SmsChurch))
                {
                    Session[SessionVariable.CellPhoneNos] = GroupDataAccessor.FetchGroupCellPhoneNos(currentPerson, groupId, selectedIds, selectedOnly);
                    noNos = ((List<string>)Session[SessionVariable.CellPhoneNos]).Count;
                }
                else
                {
                    message = ExceptionMessage.InvalidCredentials;
                }
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message,
                NoNos = noNos
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult FetchGroupEmails(int groupId, List<int> selectedIds, bool selectedOnly)
        {
            bool sessionTimedOut = false;
            string message = string.Empty;
            List<string> addresses = new List<string>();
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
                message = ExceptionMessage.SessionTimedOut;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(Permissions.EmailGroupMembers))
                {
                    Session[SessionVariable.EmailAddresses] = GroupDataAccessor.FetchGroupAddresses(currentPerson, groupId, selectedIds, selectedOnly);
                }
                else
                {
                    message = ExceptionMessage.InvalidCredentials;
                }
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult SendGroupEmail(string subject, string body)
        {
            if (Session[SessionVariable.EmailAddresses] == null)
            {
                var errorResponse = new
                {
                    SessionTimeOut = false,
                    Message = "No Email addresses selected"
                };

                return Json(errorResponse, JsonRequestBehavior.DenyGet);
            }

            List<string> addressList = (List<string>)Session[SessionVariable.EmailAddresses];
            bool sessionTimedOut = false;
            string message = string.Empty;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
                message = ExceptionMessage.SessionTimedOut;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(Permissions.EmailGroupMembers))
                {
                    message = Email.SendGroupEmail(addressList, subject, body, currentPerson);
                }
                else
                {
                    message = ExceptionMessage.InvalidCredentials;
                }
            }

            Session[SessionVariable.EmailAddresses] = null;
            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult SendGroupSms(string message)
        {
            if (Session[SessionVariable.CellPhoneNos] == null)
            {
                var errorResponse = new
                {
                    SessionTimeOut = false,
                    Message = "No CellPhoneNos selected"
                };

                return Json(errorResponse, JsonRequestBehavior.DenyGet);
            }

            List<string> chellPhoneNoList = (List<string>)Session[SessionVariable.CellPhoneNos];
            bool sessionTimedOut = false;
            string responseMessage = string.Empty;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
                responseMessage = ExceptionMessage.SessionTimedOut;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(Permissions.SmsGroupMembers))
                {
                    string username = string.Empty;
                    string password = string.Empty;
                    ChurchDataAccessor.FetchBulkSmsUsernameAndPassword(currentPerson, out username, out password);
                    if (username == null || password == null)
                    {
                        responseMessage = "Could not find Sms Provider credentials.  Please set them first in the settings tab";
                    }
                    else
                    {
                        responseMessage = Sms.SendSmses(chellPhoneNoList, message, username, password);
                    }
                }
                else
                {
                    responseMessage = ExceptionMessage.InvalidCredentials;
                }
            }

            Session[SessionVariable.CellPhoneNos] = null;
            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = responseMessage
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult FetchChurchEmailAddresses(bool search, string searchField, string searchString)
        {
            bool sessionTimedOut = false;
            string message = string.Empty;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
                message = ExceptionMessage.SessionTimedOut;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(Permissions.EmailChurch))
                {
                    Session[SessionVariable.EmailAddresses] = PersonDataAccessor.FetchChurchEmailAddresses(currentPerson, search, searchField, searchString);
                }
                else
                {
                    message = ExceptionMessage.InvalidCredentials;
                }
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message
            };

            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult FetchChurchCellPhoneNos(bool search, string searchField, string searchString)
        {
            bool sessionTimedOut = false;
            string message = string.Empty;
            int noNos = 0;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
                message = ExceptionMessage.SessionTimedOut;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(Permissions.SmsChurch))
                {
                    Session[SessionVariable.CellPhoneNos] = PersonDataAccessor.FetchChurchCellPhoneNos(currentPerson, search, searchField, searchString);
                    noNos = ((List<string>)Session[SessionVariable.CellPhoneNos]).Count;
                }
                else
                {
                    message = ExceptionMessage.InvalidCredentials;
                }
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message,
                NoNos = noNos
            };

            return Json(response, JsonRequestBehavior.DenyGet);
        }
        

        public JsonResult SavePersonComment(int personId, string comment)
        {
            bool sessionTimedOut = false;
            string message = string.Empty;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
                message = ExceptionMessage.SessionTimedOut;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(Permissions.AddComment))
                {
                    EventDataAccessor.SavePersonComment(personId, comment, currentPerson);
                    message = "Comment Saved";
                }
                else
                {
                    message = ExceptionMessage.InvalidCredentials;
                }
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult FetchPersonCommentHistory(int personId)
        {
            bool sessionTimedOut = false;
            string message = string.Empty;
            List<EventListModel> comments = new List<EventListModel>();
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
                message = ExceptionMessage.SessionTimedOut;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(Permissions.ViewGeneralComments) || currentPerson.HasPermission(Permissions.ViewPersonalComments))
                {
                    comments = EventDataAccessor.FetchCommentHistory(personId, currentPerson);
                }
                else
                {
                    message = ExceptionMessage.InvalidCredentials;
                }
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message,
                Comments = comments
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public JsonResult FetchChurchEmailTemplate(int churchId, int emailTemplateId)
        {
            bool sessionTimedOut = false;
            string message = string.Empty;
            string emailTemplate = string.Empty;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
                message = ExceptionMessage.SessionTimedOut;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(Permissions.EditEmailTemplates))
                {
                    emailTemplate = SettingsDataAccessor.FetchChurchEmailTemplate(currentPerson, churchId, emailTemplateId);
                }
                else
                {
                    message = ExceptionMessage.InvalidCredentials;
                }
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message,
                EmailTemplate = emailTemplate
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        [ValidateInput(false)]
        public JsonResult SaveChurchEmailTemplate(int churchId, int emailTemplateId, string template)
        {
            bool sessionTimedOut = false;
            string message = string.Empty;
            if (Session[SessionVariable.LoggedOnPerson] == null)
            {
                sessionTimedOut = true;
                message = ExceptionMessage.SessionTimedOut;
            }
            else
            {
                Person currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
                if (currentPerson.HasPermission(Permissions.EditEmailTemplates))
                {
                    SettingsDataAccessor.SaveChurchEmailTemplate(currentPerson, churchId, emailTemplateId, template);
                }
                else
                {
                    message = ExceptionMessage.InvalidCredentials;
                }
            }

            var response = new
            {
                SessionTimeOut = sessionTimedOut,
                Message = message
            };
            return Json(response, JsonRequestBehavior.DenyGet);
        }
        
        

        #region Private Methods

        private void SelectNewChurch(int churchId, Person currentPerson)
        {
            var newChurch = PersonDataAccessor.SelectNewChurch(currentPerson, churchId);
            if (newChurch != null)
            {
                currentPerson.ChurchId = newChurch.ChurchId;
                currentPerson.Church = newChurch;
                Session[SessionVariable.LoggedOnPerson] = currentPerson;
                Session[SessionVariable.Church] = ChurchDataAccessor.FetchChurch(churchId);
            }
        }

        private void SearchForFacebookId(int personId, string firstname, string surname, FacebookClient client)
        {
            try
            {
                string fullname = firstname + " " + surname;
                dynamic friends = client.Query("SELECT uid, first_name, last_name FROM user WHERE uid IN (    SELECT uid2    FROM friend    WHERE uid1=me()) AND name='" + fullname + "'");
                if (friends.Count == 1)
                {
                    PersonDataAccessor.SavePersonFacebookId(personId, friends[0].uid);
                }
            }
            catch { }

        }
        #endregion Private Methods
    }
}
